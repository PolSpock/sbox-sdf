﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Sdf;

namespace Sandbox.MarchingSquares
{
    public partial class MarchingSquaresChunk : ModelEntity
    {
        private Mesh _mesh;

        private SdfArray2D Data { get; set; }

        public MarchingSquaresChunk()
        {

        }

        public MarchingSquaresChunk( int resolution, float size, float? maxDistance = null )
        {
            Data = new SdfArray2D( resolution, size, maxDistance ?? (size * 4f / resolution), NormalStyle.Flat );
        }

        public void Clear( MarchingSquaresMaterial material = null )
        {
            Data.Clear( material );
        }

        public bool Add<T>( in T sdf, MarchingSquaresMaterial material )
            where T : ISdf2D
        {
            return Data.Add( in sdf, material );
        }

        public bool Subtract<T>( in T sdf )
            where T : ISdf2D
        {
            return Data.Subtract( in sdf );
        }

        public void UpdateMesh()
        {
            var writer = new MarchingSquaresMeshWriter();

            Data.WriteTo( writer, Data.Materials.First() );

            _mesh ??= new Mesh( Data.Materials.First().FrontFaceMaterial );

            writer.ApplyTo( _mesh );

            if ( Model == null )
            {
                var builder = new ModelBuilder();

                builder.AddMesh( _mesh );

                Model = builder.Create();
            }
        }
    }
}
