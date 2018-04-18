using MarchingCubes.Algoritms.CountorLines;
using MarchingCubes.Algoritms.MarchingCubes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MarchingCubesDemo.WPF
{
    /// <summary>
    /// Convert list of triangles to 3d model.
    /// </summary>
    public class TrianglesConvertor
    {
        public ModelVisual3D Convert(List<Triangle> triangles,
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
                //Draw normals
                //mesh.TriangleIndices.Add(index);
                //mesh.TriangleIndices.Add(++index);
                //mesh.TriangleIndices.Add(++index);
                //index++;
                //if (isMeshSmooth)
                //{
                //    mesh.Normals.Add(triangle.Point1.NormalVector);
                //    mesh.Normals.Add(triangle.Point2.NormalVector);
                //    mesh.Normals.Add(triangle.Point3.NormalVector);
                //}
                //if (!triangle.IsInvalid)
                //{
                //    mesh.Positions.Add(triangle.Point1.ToPoint3D());
                //    mesh.Positions.Add(triangle.Point2.ToPoint3D());
                //    mesh.Positions.Add(triangle.Point3.ToPoint3D());
                //}
            }
            //Arguments previous = null;
            //int index2 = 0;
            //foreach (var test in normals)
            //{
            //    if (previous == null)
            //        previous = test.Point;
            //    if (Object.ReferenceEquals(test.Point, previous))
            //    {
            //        index2++;
            //        continue;
            //    }
            //    else
            //    {
            //        break;
            //    }

            //}

            // Create MaterialGroup.
            var matgrp = new MaterialGroup();
            matgrp.Children.Add(new DiffuseMaterial(Brushes.BlueViolet));
            matgrp.Children.Add(new SpecularMaterial(Brushes.LightBlue, 24));

            // Create GeometryModel3D.
            var model = new GeometryModel3D(mesh, matgrp);

            modelToAdd.Content = model;
            return modelToAdd;
        }
    }
}
