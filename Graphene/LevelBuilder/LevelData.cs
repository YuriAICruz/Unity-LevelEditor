using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Graphene.Grid;
using UnityEngine;

namespace Graphene.LevelBuilder
{
    [Serializable]
    public class LevelItem
    {
        public int Id;
        public Vector2Int Pos;

        public LevelItem(int id, Vector2Int pos)
        {
            Id = id;
            Pos = pos;
        }
    }

    [RequireComponent(typeof(GridSystem))]
    public class LevelData : MonoBehaviour
    {
        [Tooltip("From the Resources Folder")] public string PatternPath = "Level/LevelPattern";

        public List<LevelItem> LevelItems;

        public List<Color> Colors;

        public int CurrentId;

        private GridSystem _grid;

        private void Awake()
        {
            _grid = GetComponent<GridSystem>();
        }

        public void AddItem(IGridInfo gr)
        {
            if (LevelItems == null)
            {
                LevelItems = new List<LevelItem>();
            }

            if (LevelItems.Exists(x => x.Pos.x == gr.x && x.Pos.y == gr.y))
            {
                return;
            }

            LevelItems.Add(new LevelItem(CurrentId, new Vector2Int(gr.x, gr.y)));
        }

        public void RemoveItem(IGridInfo gr)
        {
            if (LevelItems == null)
            {
                LevelItems = new List<LevelItem>();
            }

            var i = LevelItems.FindIndex(x => x.Pos.x == gr.x && x.Pos.y == gr.y);
            if (i >= 0)
            {
                LevelItems.RemoveAt(i);
            }
        }

        public void Build()
        {
            var pattern = Resources.Load<LevelItensPattern>(PatternPath);

            if (pattern == null)
            {
                Debug.LogError("Not found pattern on Resources/" + PatternPath + ".assets");
            }
            if (_grid == null)
                _grid = GetComponent<GridSystem>();

            for (var i = transform.childCount-1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            foreach (var item in LevelItems)
            {
                var cell = _grid.Grid.GetPos(item.Pos.x, item.Pos.y);
                var i = pattern.Patterns.FindIndex(x => x.Id == item.Id);
                if (i >= 0)
                {
                    var tmp = Instantiate(pattern.Patterns[i].MatchingSquares[4], cell.worldPos, Quaternion.identity, transform);
                }
            }
        }
    }
}