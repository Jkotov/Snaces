using UnityEngine;

namespace Components
{
    public struct CollisionEntitiesEventComponent
    {
        public PackedEntity entity;
        public PackedEntity other;
        public GameObject gameObject;
    }
}