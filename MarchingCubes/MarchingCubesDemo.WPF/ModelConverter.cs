using MarchingCubes.Algoritms.CountorLines;
using MarchingCubes.Algoritms.MarchingCubes;
using Petzold.Media3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MarchingCubesDemo.WPF
{
    /// <summary>
    /// Convert list of triangles to 3d model.
    /// </summary>
    public class ModelConverter
    {
        public ModelVisual3D RenderMesh(List<Triangle> triangles,
            bool isMeshSmooth)
        {
            //countor line
            var modelToAdd = new ModelVisual3D();
            // Create MeshGeometry3D.
            var mesh = new MeshGeometry3D();
            int index = 0;
            var testPoint = new Point3D(0.5, 1, 2);
            //List<Test> normals = new List<Test>();
            foreach (var triangle in triangles)
            {
               // Draw normals
                mesh.TriangleIndices.Add(index);
                mesh.TriangleIndices.Add(++index);
                mesh.TriangleIndices.Add(++index);
                index++;

                if (isMeshSmooth)
                {
                    //mesh.Normals.Add(ConvertToVector(triangle.Point1.NormalVector));
                    //mesh.Normals.Add(ConvertToVector(triangle.Point2.NormalVector));
                    //mesh.Normals.Add(ConvertToVector(triangle.Point3.NormalVector));
                }

                //if (!triangle.IsInvalid)
                {
                    mesh.Positions.Add(Convert(triangle.Point1));
                    mesh.Positions.Add(Convert(triangle.Point2));
                    mesh.Positions.Add(Convert(triangle.Point3));
                }
            }

            // Create MaterialGroup.
            var matgrp = new MaterialGroup();
            matgrp.Children.Add(new DiffuseMaterial(Brushes.BlueViolet));
            matgrp.Children.Add(new SpecularMaterial(Brushes.LightBlue, 24));

            // Create GeometryModel3D.
            var model = new GeometryModel3D(mesh, matgrp);

            modelToAdd.Content = model;
            return modelToAdd;
        }

        public Vector3D ConvertToVector(MarchingCubes.CommonTypes.Point point)
        {
            return new Vector3D(point.X, point.Y, point.Z);
        }

        public Point3D Convert(MarchingCubes.CommonTypes.Point point)
        {
            return new Point3D(point.X, point.Y, point.Z);
        }

        public ModelVisual3D RenderGrid(List<GridCube> cubes, bool drawVertextIndexes,  int? index = null)
        {
            var modelToAdd = new ModelVisual3D();


            var lines = new List<GridLine>();
            //foreach (var cube in cubes)

            if (index.HasValue)
            {
                if (index <= 0)
                {
                    index = 0;
                }
                else if (cubes.Count <= index)
                {
                    index = 0;
                }
            }

            var unique = new List<MarchingCubes.CommonTypes.Point>();
            int globalIndex = 0;
            foreach (var cube in cubes)
            {
                var toUseCube = cube;
                if (index.HasValue)
                {
                    toUseCube = cubes[index.Value];
                }

                foreach (var edge in toUseCube.Edges)
                {
                    if (!lines.Contains(edge))
                    {
                        lines.Add(edge);
                        modelToAdd.Children.Add(GetLine(edge.Point1, edge.Point2, Colors.Red));
                    }
                }

                if (drawVertextIndexes)
                {
                    foreach (var vert in toUseCube.Vertex)
                    {
                        if (!unique.Contains(vert))
                        {
                            var model = new Petzold.Media3D.WireText() { Text = globalIndex.ToString(), };
                            model.Origin = new Point3D(vert.X, vert.Y, vert.Z);
                            model.FontSize = 1;
                            modelToAdd.Children.Add(model);
                            globalIndex++;
                            unique.Add(vert);
                        }
                    }

                    if (index.HasValue)
                    {
                        int indexName = 0;
                        foreach (var vert in toUseCube.Vertex)
                        {
                            var model = new Petzold.Media3D.WireText() { Text = indexName.ToString(), };
                            model.Origin = new Point3D(vert.X, vert.Y, vert.Z);
                            model.FontSize = 1;
                            modelToAdd.Children.Add(model);
                            indexName++;
                        }
                        break;
                    }
                }
            }

            return modelToAdd;
        }

        private WireLine GetLine(MarchingCubes.CommonTypes.Point point1, MarchingCubes.CommonTypes.Point point2, Color color)
        {
            var line = new WireLine();
            line.Point1 = new Point3D(point1.X, point1.Y, point1.Z);
            line.Point2 = new Point3D(point2.X, point2.Y, point2.Z);
            line.Thickness = 1;
            line.Color = color;
            return line;
        }

    }
}
