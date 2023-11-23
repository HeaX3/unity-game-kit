﻿using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace GameKit.Entities
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnityPathfindingEntity : MonoBehaviour, IPathfindingEntity
    {
        [SerializeField] [HideInInspector] private NavMeshAgent agent;

        public Vector3[] path => agent.path?.corners;
        
        public float speed
        {
            get => agent.speed;
            set => agent.speed = value;
        }

        public void Initialize(EntityController controller)
        {
            
        }

        public void ApplyEntity(IEntity entity)
        {
            
        }
        
        public bool SetDestination(Vector3 position)
        {
            return agent.SetDestination(position);
        }

        public void Cancel()
        {
            agent.ResetPath();
        }

        private void OnValidate()
        {
            if (!agent || agent.gameObject != gameObject) agent = GetComponent<NavMeshAgent>();
        }
    }
}