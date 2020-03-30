using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrblCNC.Glutils;

namespace GrblCNC
{
    class GCodeDimensions
    {
        Line3D lines;
        Text3D texts;

        const int XColor = 0x20C020;
        const int YColor = 0xC02020;
        const int ZColor = 0x2020C0;

        public float fontsize = 2; // mm

        public GCodeDimensions()
        {
            lines = new Line3D();
            texts = new Text3D(FontManager.BitmapFont);
        }

        public void Clear()
        {
            lines.Clear();
            texts.Clear();
        }

        string CalcPosition(float minVal, float maxVal, out float pos)
        {
            float len = maxVal - minVal;
            string stlen = len.ToString("0.00");
            float stdim = fontsize * stlen.Length;
            pos = (len - stdim) / 2;
            return stlen;
        }

        public void Init(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            float pos;
            string stlen;
            Clear();

            // X lines
            lines.AddLine(minX, minY - 1, minZ, minX, minY - 5, minZ, XColor);
            lines.AddLine(maxX, minY - 1, minZ, maxX, minY - 5, minZ, XColor);
            lines.AddLine(minX, minY - 3, minZ, maxX, minY - 3, minZ, XColor);
            stlen = CalcPosition(minX, maxX, out pos);
            texts.AddText(stlen, Obj3DText.TextPlane.XY, fontsize, minX + pos, minY - 3, minZ, 0, XColor.ToString("X"));

            // Y lines
            lines.AddLine(minX - 1, minY, minZ, minX - 5, minY, minZ, YColor);
            lines.AddLine(minX - 1, maxY, minZ, minX - 5, maxY, minZ, YColor);
            lines.AddLine(minX - 3, minY, minZ, minX - 3, maxY, minZ, YColor);
            stlen = CalcPosition(minY, maxY, out pos);
            texts.AddText(stlen, Obj3DText.TextPlane.XY, fontsize, minX - 3, maxY - pos, minZ, 90, YColor.ToString("X"));
            
            // Z lines
            lines.AddLine(minX - 1, minY, minZ, minX - 5, minY, minZ, ZColor);
            lines.AddLine(minX - 1, minY, maxZ, minX - 5, minY, maxZ, ZColor);
            lines.AddLine(minX - 3, minY, minZ, minX - 3, minY, maxZ, ZColor);
            stlen = CalcPosition(minZ, maxZ, out pos);
            texts.AddText(stlen, Obj3DText.TextPlane.XZ, fontsize, minX - 3, minY, maxZ - pos, 90, ZColor.ToString("X"));

            lines.Init();
        }

        public void Render()
        {
            lines.Render();
            texts.Render();
        }

    }
}
