using InsaneOne.EcsRts.UI;
using Leopotam.Ecs;
using UnityEngine;

namespace InsaneOne.EcsRts
{
    sealed class SelectionSystem : IEcsRunSystem 
    {
        readonly EcsWorld world = null;
        readonly Camera camera;
        
        readonly EcsFilter<UnitComponent> unitsFilter = null;
        readonly EcsFilter<SelectedTag, UnitComponent> selectedFilter = null;
        
        void IEcsRunSystem.Run()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            foreach (var i in selectedFilter)
            {
                ref var unit = ref selectedFilter.Get2(i);

                world.NewEntity().Get<RemoveHealthbarEvent>().FromUnit = unit;
                
                selectedFilter.GetEntity(i).Del<SelectedTag>();
            }

            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, 1000))
            {
                foreach (var i in unitsFilter)
                {
                    ref var unitComponent = ref unitsFilter.Get1(i);
                    var unitObject = unitComponent.SelfObject;

                    if (hit.collider.gameObject == unitObject && unitComponent.OwnerPlayerId == PlayerComponent.LocalPlayerId)
                    {
                        var entity = unitsFilter.GetEntity(i);

                        entity.Get<SelectedTag>();
                        
                        world.NewEntity().Get<AddHealthbarEvent>().ToUnit = unitComponent;
                    }
                }
            }
        }
    }
}