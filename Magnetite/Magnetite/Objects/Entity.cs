using System;
using UnityEngine;

namespace Magnetite
{
	public class Entity
	{
		#region Variables

		public readonly BaseEntity baseEntity;

		public Vector3 Location {
			get {
				return baseEntity.transform.position;
			}
			set {
				baseEntity.transform.position.Set(value.x, value.y, value.z);
			}
		}

		public string Name {
			get
			{
				return baseEntity.sourcePrefab.Substring(baseEntity.sourcePrefab.LastIndexOf("/"));
			}
		}

		public float X {
			get {
				return baseEntity.transform.position.x;
			}
			set {
				baseEntity.transform.position.Set(value, Y, Z);
			}
		}

		public float Y {
			get {
				return baseEntity.transform.position.y;
			}
			set {
				baseEntity.transform.position.Set(X, value, Z);
			}
		}

		public float Z {
			get {
				return baseEntity.transform.position.z;
			}
			set {
				baseEntity.transform.position.Set(X, Y, value);
			}
		}

		#endregion

		public Entity(BaseEntity ent)
		{
			baseEntity = ent;
		}
	}
}
