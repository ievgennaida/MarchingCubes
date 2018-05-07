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
    public class ModelsConverter
    {
        public ModelVisual3D RenderMesh(
            List<Triangle> triangles,
            bool useNormals, 
            bool showNormalVectors, 
            bool doubleSided)
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
                AddTriangle(mesh, triangle.Point1, triangle.Point2, triangle.Point3, index, useNormals);
                index += 3;
            }

            if (showNormalVectors)
            {
                var i = 0;
                foreach (var normal in mesh.Normals)
                {
                    var point = mesh.Positions[i];
                    var p = Convert(mesh.Positions[i]);


                    // var point = Vector3D.Add(,point.ad//normal.adnew MarchingCubes.CommonTypes.Point(b.X, b.Y, b.Z);
                    modelToAdd.Children.Add(GetLine(p, new MarchingCubes.CommonTypes.Point(normal.X, normal.Y, normal.Z), Colors.Blue));
                    i++;
                }
            }

            if (doubleSided)
            {
                foreach (var triangle in triangles)
                {
                    AddTriangle(mesh, triangle.Point3, triangle.Point2, triangle.Point1, index, useNormals);
                    index += 3;
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

        private int AddTriangle(MeshGeometry3D mesh,
            MarchingCubes.CommonTypes.Point p1, 
            MarchingCubes.CommonTypes.Point p2,
            MarchingCubes.CommonTypes.Point p3, 
            int index, 
            bool useNormalVectors)
        {
            // Draw normals
            mesh.TriangleIndices.Add(index);
            mesh.TriangleIndices.Add(++index);
            mesh.TriangleIndices.Add(++index);

            if (useNormalVectors)
            {
                mesh.Normals.Add(ConvertToVector(p1.NormalVector));
                mesh.Normals.Add(ConvertToVector(p2.NormalVector));
                mesh.Normals.Add(ConvertToVector(p3.NormalVector));
            }

            mesh.Positions.Add(Convert(p1));
            mesh.Positions.Add(Convert(p2));
            mesh.Positions.Add(Convert(p3));
            return index;
        }


        public static void AddNormals(Triangle triangle, Vector3D normal)
        {
           
        }

        public Vector3D ConvertToVector(MarchingCubes.CommonTypes.Point point)
        {
            return new Vector3D(point.X, point.Y, point.Z);
        }

        public MarchingCubes.CommonTypes.Point Convert(Point3D point)
        {
            return new MarchingCubes.CommonTypes.Point(point.X, point.Y, point.Z);
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
