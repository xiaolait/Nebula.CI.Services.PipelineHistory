using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nebula.CI.Services.PipelineHistory
{
    public class Digram
    {
        public string Name { get; set; }
        public List<Node> NodeList { get; set; } = new List<Node>();
        public List<Line> LineList { get; set; } = new List<Line>();

        public static Digram CreateInstance(string json)
        {
            var digram = JsonConvert.DeserializeObject<Digram>(json);
            foreach (var l in digram.LineList)
            {
                foreach (var n in digram.NodeList)
                {
                    if (l.From == n.Id)
                    {
                        n.Destination.Add(l.To);
                    }
                    if (l.To == n.Id)
                    {
                        n.Source.Add(l.From);
                    }
                }
            }
            return digram;
        }
    }

    public class Node
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string AnnoName { get; set; }
        public string ConfigUrl { get; set; }
        public string ResultUrl { get; set; }
        public string Left { get; set; }
        public string Top { get; set; }
        public Property Property { get; set; }
        public string Ico { get; set; }
        public List<string> Source { get; set; } = new List<string>();
        public List<string> Destination { get; set; } = new List<string>();
    }

    public class Line
    {
        public string From { get; set; }
        public string To { get; set; }
    }

    public class Property
    {
        public List<TaskParam> Params { get; set; } = new List<TaskParam>();
        public Resource Resources { get; set; }
    }
    public class TaskParam
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class Resource
    {
        public List<ResourceDetail> Inputs { get; set; } = new List<ResourceDetail>();
        public List<ResourceDetail> Outputs { get; set; } = new List<ResourceDetail>();
    }
    public class ResourceDetail
    {
        public string Name { get; set; }
        public string Resource { get; set; }
    }
}
