using System;

namespace _Scripts.State
{
    public class Transition {
        public State Origin { get; set; }
        public State Target { get; set; }
        public Func<bool> Predicate { get; set; }
    }
}