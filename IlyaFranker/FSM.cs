using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IlyaFranker
{
    /// <summary>
    /// TODO: this is a utility class (and quite a useful one too) so move it to commons
    /// </summary>
    /// <typeparam name="TEdge"></typeparam>
    public class FSM<TEdge>
    {
        private FsmNode _lastAddedNode;
        private FsmAdvanceArgs args;
        private Dictionary<string, FsmNode> nodes;
        private FsmNode curNode;

        public FSM() {
            nodes = new Dictionary<string, FsmNode>();
            args = new FsmAdvanceArgs();
        }

        public FSM<TEdge> addNode_0_(string key) {
            _lastAddedNode = new FsmNode() {
                Key = key,
            };
            nodes.Add(key, _lastAddedNode);
            return this;
        }
        public FSM<TEdge> addEdge___(TEdge trigger, string destination, Action<FsmAdvanceArgs> callback) {
            if (_lastAddedNode == null)
                throw new Exception("Must call addNode before calling addEdge");
            _lastAddedNode.Transitions.Add(trigger, new FsmEdge() {
                Callback = callback,
                Destination = destination,
            });
            return this;
        }

        public void Goto(string key) {
            curNode = null;
            if (nodes.ContainsKey(key)) {
                curNode = nodes[key];
            }
        }
        public void Reset() {
            curNode = null;
        }

        public void Advance(TEdge action) {
            if (curNode == null)
                return;
            if (!curNode.Transitions.ContainsKey(action))
                return;
            var edge = curNode.Transitions[action];
            if (!nodes.ContainsKey(edge.Destination))
                throw new Exception("Incorrect graph. Cannot go to key " + edge.Destination);
            args.Destination = edge.Destination;
            edge.Callback(args);
            Goto(args.Destination);
        }


        //-----------------

        private class FsmNode {
            public string Key { get; set; }
            public Dictionary<TEdge, FsmEdge> Transitions { get; private set; }
            public FsmNode() {
                Transitions = new Dictionary<TEdge, FsmEdge>();
            }
        }
        private class FsmEdge {
            public string Destination { get; set; }
            public Action<FsmAdvanceArgs> Callback { get; set; }
            public FsmEdge() {}
        }
        
        

    }

    public class FsmAdvanceArgs {
        public string Destination { get; set; }
    }
}
