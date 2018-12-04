using System.Collections.Generic;
using Graphene.Grid;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Graphene.LevelBuilder
{
    [CustomEditor(typeof(LevelData))]
    public class LevelDataInspector : Editor
    {
        private LevelData _self;
        private GridSystem _grid;
        private Camera _camera;

        private void Awake()
        {
            _self = target as LevelData;
            _grid = _self.GetComponent<GridSystem>();

            _camera = Camera.current;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Build"))
            {
                _self.Build();
            }
        }

        private void OnSceneGUI()
        {
            _camera = Camera.current;

            GridSelector();
        }

        private void GridSelector()
        {
            var mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            var pos = new Vector3(mouse.x, mouse.y - 104, _camera.nearClipPlane);
            var ray = HandleUtility.GUIPointToWorldRay(pos);
            
            var cell = _grid.Grid.GetPos(ray);
            
            if (cell != null)
            {
                var color = Handles.color;
                Handles.color = Color.magenta;
                
                var sqr = cell.GetEdges();

                for (int i = 0, n = sqr.Length; i < n; i++)
                {
                    Handles.DrawLine(sqr[i], sqr[(i + 1) % n]);
                }
            
                if(Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    _self.AddItem(cell);
                    Undo.RecordObject(target, "Add Level Item");
                    EditorUtility.SetDirty(_self);
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                }
                if(Event.current.type == EventType.MouseDown && Event.current.button == 1)
                {
                    _self.RemoveItem(cell);
                    Undo.RecordObject(target, "Remove Level Item");
                    EditorUtility.SetDirty(_self);
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                }
                
                Handles.color = color;
                
                SceneView.RepaintAll();
            }

            var p = ray.GetPoint(10);
            
            Handles.DrawWireDisc(p, _camera.transform.forward, 0.1f);

            DrawLevelInfo();
        }

        private void DrawLevelInfo()
        {
            if (_self.LevelItems == null)
            {
                _self.LevelItems = new List<LevelItem>();
            }
            
            var color = Handles.color;
            
            foreach (var item in _self.LevelItems)
            {
                var c = Color.white;
                if(item.Id < _self.Colors.Count)
                    c = _self.Colors[item.Id];
                else
                {
                    _self.Colors.Add(Color.white);
                }

                var cell = _grid.Grid.GetPos(item.Pos.x, item.Pos.y);
                Handles.DrawSolidRectangleWithOutline(cell.GetEdges(), c, c);
            }
            
            Handles.color = color;
        }
    }
}