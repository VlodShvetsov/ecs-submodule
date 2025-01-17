﻿
namespace ME.ECS {

    public static class TransformComponentsInitializer {

        public static void InitTypeId() {

            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Nodes>();
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Container>();
            
            Transform2DComponentsInitializer.InitTypeId();
            Transform3DComponentsInitializer.InitTypeId();

        }

        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            structComponentsContainer.Validate<ME.ECS.Transform.Nodes>();
            structComponentsContainer.Validate<ME.ECS.Transform.Container>();

            Transform2DComponentsInitializer.Init(ref structComponentsContainer);
            Transform3DComponentsInitializer.Init(ref structComponentsContainer);

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateData<ME.ECS.Transform.Nodes>();
            entity.ValidateData<ME.ECS.Transform.Container>();
            
            Transform2DComponentsInitializer.Init(in entity);
            Transform3DComponentsInitializer.Init(in entity);

        }

    }

}
