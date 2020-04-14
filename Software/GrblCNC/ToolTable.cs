using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GrblCNC
{
    public class ToolTable
    {
        List<CncTool> tools;

        public ToolTable()
        {
            tools = new List<CncTool>();
        }

        public List<CncTool> Tools
        {
            get { return tools; }
        }

        public CncTool GetTool(int toolNum)
        {
            foreach (CncTool tool in tools)
                if (toolNum == tool.toolNum)
                    return tool;
            return null;
        }

        public void AddUpdateTool(CncTool updatetool)
        {
            CncTool tool = GetTool(updatetool.toolNum);
            if (tool == null)
            {
                tool = new CncTool(updatetool.toolNum);
                tools.Add(tool);
                tools.Sort();
            }
            tool.CopyFrom(updatetool);
        }

        public void RemoveTool(int toolnum)
        {
            CncTool tool = GetTool(toolnum);
            if (tool != null)
                tools.Remove(tool);
        }

        public void Clear()
        {
            tools.Clear();
        }

        public void UpdateFrom(ToolTable otherTable)
        {
            foreach (CncTool tool in otherTable.tools)
                AddUpdateTool(tool);
        }

        public string Save(string filepath)
        {
            try
            {
                StreamWriter sw = new StreamWriter(filepath);
                sw.WriteLine("# GrbleCNC Tool Table [1.0]");
                foreach (CncTool tool in tools)
                {
                    sw.WriteLine(tool.ToString());
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "OK";
        }


        public string Load(string filepath)
        {
            try
            {
                StreamReader sr = new StreamReader(filepath);
                string line = sr.ReadLine().Trim();
                if (!line.StartsWith("# GrbleCNC Tool Table"))
                    return "Not a GrblCNC Tool Table file";
                tools.Clear();
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine().Trim();
                    if (line.Length == 0 || line[0] == '#')
                        continue;
                    CncTool tool = new CncTool(0);
                    if (tool.TryParse(line).Count == 0)
                    {
                        tool.Parse(line);
                        tools.Add(tool);
                    }
                }
                sr.Close();
                if (tools.Count > 0)
                    tools.Sort();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "OK";
        }
    }
}
