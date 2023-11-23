
using System;                           // for Serializable
using System.Runtime.InteropServices;   // for StructLayout

using System.Net;
using System.Net.Sockets;
using System.Collections;               // for ArrayList

namespace Redgate.Net.mDNS {

    class Label {
        string label;

        public byte
	Length {
            get { return (byte)label.Length; }
        }

        public Label( string s ) {
            // check for '.'
            label = s;
        }

        public byte[]
	GetBytes() {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes( label );
        }
    }

    class PacketBuilder {
        ArrayList list = new ArrayList();

        public void
	Add( byte value ) {
            list.Add( value );
        }

        public void
	Add( ushort value ) {
            ushort n = (ushort)System.Net.IPAddress.HostToNetworkOrder( (short)value );
            list.AddRange( System.BitConverter.GetBytes(n) );
        }

        public void
	Add( byte[] bytes ) {
            list.AddRange( bytes );
        }

        public void
	Add( Label label ) {
            Add( label.Length );
            Add( label.GetBytes() );
        }

        public void
	Add( string domain ) {
            char[] dots = {'.'};
            string [] labels = domain.Trim(dots).Split(dots);
            foreach ( string label in labels ) {
                Add( new Label(label) );
            }
	    Add( (byte)0 );  // end the name
        }

        public void
	Add( PacketBuilder packet ) {
            list.AddRange( packet.GetBytes() );
        }

        public byte[]
	GetBytes() {
            byte[] bytes = new byte[list.Count];
            list.CopyTo( bytes );
            return bytes;
        }

    }

    class PacketParser {
	byte[] data;
    string[] stData;
	int here = 0;
	int dot;
	int remaining;
	int end;

        public PacketParser( byte[] bytes ) {
	    data = bytes;
            stData = new string[bytes.Length];
            for (int i = 0; i < bytes.Length; i++) { 
                int c = data[i];
                if (c >= ' ' && c <= 'z')
                    stData[i] = (char)c + " ";
                else
                    stData[i] = ". ";
                stData[i] += c.ToString("X");
            }
	    end = bytes.Length;
	    remaining = end;
	}

	private void DumpPacket() {
	    System.Console.WriteLine( "packet dump: " );
	    System.Console.WriteLine( System.BitConverter.ToString(data) );
	    System.Console.WriteLine( "here {0}  dot {1}  end {2}", here, dot, end );
	}

	private void
	forward( int n ) {
	    if ( remaining < n ) {
	        throw new System.IndexOutOfRangeException("No more data in packet");
	    }
	    here += n;
	    remaining -= n;
	}

	public byte
	PopByte() {
	    return data[here++];
	}

	public ushort
	PopUShort() {
	    ushort value = System.BitConverter.ToUInt16( data, here );
	    forward( 2 );
            return (ushort)System.Net.IPAddress.NetworkToHostOrder( (short)value );
	}

	public int
	PopInt() {
	    int value = System.BitConverter.ToInt32( data, here );
	    forward( 4 );
            return (int)System.Net.IPAddress.NetworkToHostOrder( (int)value );
	}

	public byte[]
	PopBytes( int count ) {
	    byte[] value = new byte[count];
	    System.Array.Copy( data, here, value, 0, count );
	    forward( count );
            return value;
	}

	public string
	PopString() {
	    int length = PopByte();
	    System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
	    if ( length > remaining ) {
	        System.Console.WriteLine( "PopString: length {0}, remaining {1} - truncating", length, remaining );
		length = remaining;
	    }
	    string value = encoder.GetString( data, here, length );
	    forward( length );
            return value;
	}

	public int
	GetPointerLength() {
	    if ( dot > end ) {
		DumpPacket();
	        throw new System.IndexOutOfRangeException("read past end of packet " + dot + "/" + end);
	    }
	    int length = data[dot++];
	    if ( (length & 0xc0) != 0xc0 ) return length;
	    dot = ((length & 0x3f) << 8) + data[dot];
	    return GetPointerLength();
	}

	public int
	PopLabelLength() {
	    if ( dot != 0 ) return GetPointerLength();
	    int length = data[here++];
	    if ( (length & 0xc0) != 0xc0 ) return length;
	    dot = ((length & 0x3f) << 8) + data[here++];
	    return GetPointerLength();
	}

	public string
	GetPointer( int length ) {
	    if ( length == 0 ) return null;

	    System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
	    string label = encoder.GetString( data, dot, length );

	    dot += length;
            return label;
	}

	public string
	PopLabel() {
	    int length = PopLabelLength();
	    if ( length == 0 ) return null;

	    if ( dot != 0 ) return GetPointer( length );

	    System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
	    string label = encoder.GetString( data, here, length );
	    forward( length );

            return label;
	}

	public string
	PopDomain() {
	    string domain = "";
	    dot = 0;

	    while ( true ) {
	        string label = null;
		try {
	            label = PopLabel();
		} catch ( System.IndexOutOfRangeException ) {
		    System.Console.WriteLine( "error processing " + domain );
		    throw;
		}
		if ( label == null ) break;
		domain += label + ".";
	    }

            return domain;
	}

    }

}
