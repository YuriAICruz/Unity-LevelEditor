using System;
using System.Collections.Generic;
using UnityEngine;

namespace Graphene.LevelBuilder
{
    [CreateAssetMenu(fileName = "LevelPattern", menuName = "LevelEditor/LevelPattern", order = 1)]
    public class LevelItensPattern : ScriptableObject
    {
        public List<Pattern> Patterns;
    }

    [Serializable]
    public class Pattern
    {
        public int Id;
        
        public GameObject[] MatchingSquares = new GameObject[9];
    }
}