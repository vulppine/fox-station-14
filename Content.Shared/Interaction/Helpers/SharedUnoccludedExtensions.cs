using Content.Shared.DragDrop;
using Content.Shared.Examine;
using Robust.Shared.Containers;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using static Content.Shared.Examine.ExamineSystemShared;
using static Content.Shared.Interaction.SharedInteractionSystem;

namespace Content.Shared.Interaction.Helpers
{
    public static class SharedUnoccludedExtensions
    {
        #region Entities
        public static bool InRangeUnOccluded(
            this IEntity origin,
            IEntity other,
            float range = ExamineRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            return ExamineSystemShared.InRangeUnOccluded(origin, other, range, predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this IEntity origin,
            IComponent other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            return ExamineSystemShared.InRangeUnOccluded(origin, other, range, predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this IEntity origin,
            IContainer other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var otherEntity = other.Owner;

            return ExamineSystemShared.InRangeUnOccluded(origin, otherEntity, range, predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this IEntity origin,
            EntityCoordinates other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            return ExamineSystemShared.InRangeUnOccluded(origin, other, range, predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this IEntity origin,
            MapCoordinates other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            return ExamineSystemShared.InRangeUnOccluded(origin, other, range, predicate, ignoreInsideBlocker);
        }
        #endregion

        #region Components
        public static bool InRangeUnOccluded(
            this IComponent origin,
            IEntity other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originEntity = origin.Owner;

            return ExamineSystemShared.InRangeUnOccluded(originEntity, other, range, predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this IComponent origin,
            IComponent other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originEntity = origin.Owner;

            return ExamineSystemShared.InRangeUnOccluded(originEntity, other, range, predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this IComponent origin,
            IContainer other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originEntity = origin.Owner;
            var otherEntity = other.Owner;

            return ExamineSystemShared.InRangeUnOccluded(originEntity, otherEntity, range, predicate,
                ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this IComponent origin,
            EntityCoordinates other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originEntity = origin.Owner;

            return ExamineSystemShared.InRangeUnOccluded(originEntity, other, range, predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this IComponent origin,
            MapCoordinates other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originEntity = origin.Owner;

            return ExamineSystemShared.InRangeUnOccluded(originEntity, other, range, predicate,
                ignoreInsideBlocker);
        }
        #endregion

        #region Containers
        public static bool InRangeUnOccluded(
            this IContainer origin,
            IEntity other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originEntity = origin.Owner;

            return ExamineSystemShared.InRangeUnOccluded(originEntity, other, range, predicate,
                ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this IContainer origin,
            IComponent other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originEntity = origin.Owner;

            return ExamineSystemShared.InRangeUnOccluded(originEntity, other, range, predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this IContainer origin,
            IContainer other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originEntity = origin.Owner;
            var otherEntity = other.Owner;

            return ExamineSystemShared.InRangeUnOccluded(originEntity, otherEntity, range, predicate,
                ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this IContainer origin,
            EntityCoordinates other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originEntity = origin.Owner;

            return ExamineSystemShared.InRangeUnOccluded(originEntity, other, range, predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this IContainer origin,
            MapCoordinates other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originEntity = origin.Owner;

            return ExamineSystemShared.InRangeUnOccluded(originEntity, other, range, predicate, ignoreInsideBlocker);
        }
        #endregion

        #region EntityCoordinates
        public static bool InRangeUnOccluded(
            this EntityCoordinates origin,
            IEntity other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originPosition = origin.ToMap(other.EntityManager);
            var otherPosition = other.Transform.MapPosition;

            return ExamineSystemShared.InRangeUnOccluded(originPosition, otherPosition, range,
                predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this EntityCoordinates origin,
            IComponent other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originPosition = origin.ToMap(other.Owner.EntityManager);
            var otherPosition = other.Owner.Transform.MapPosition;

            return ExamineSystemShared.InRangeUnOccluded(originPosition, otherPosition, range,
                predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this EntityCoordinates origin,
            IContainer other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var originPosition = origin.ToMap(other.Owner.EntityManager);
            var otherPosition = other.Owner.Transform.MapPosition;

            return ExamineSystemShared.InRangeUnOccluded(originPosition, otherPosition, range,
                predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this EntityCoordinates origin,
            EntityCoordinates other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true,
            IEntityManager? entityManager = null)
        {
            entityManager ??= IoCManager.Resolve<IEntityManager>();

            var originPosition = origin.ToMap(entityManager);
            var otherPosition = other.ToMap(entityManager);

            return ExamineSystemShared.InRangeUnOccluded(originPosition, otherPosition, range,
                predicate, ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this EntityCoordinates origin,
            MapCoordinates other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true,
            IEntityManager? entityManager = null)
        {
            entityManager ??= IoCManager.Resolve<IEntityManager>();

            var originPosition = origin.ToMap(entityManager);

            return ExamineSystemShared.InRangeUnOccluded(originPosition, other, range, predicate,
                ignoreInsideBlocker);
        }
        #endregion

        #region MapCoordinates
        public static bool InRangeUnOccluded(
            this MapCoordinates origin,
            IEntity other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var otherPosition = other.Transform.MapPosition;

            return ExamineSystemShared.InRangeUnOccluded(origin, otherPosition, range, predicate,
                ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this MapCoordinates origin,
            IComponent other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var otherPosition = other.Owner.Transform.MapPosition;

            return ExamineSystemShared.InRangeUnOccluded(origin, otherPosition, range, predicate,
                ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this MapCoordinates origin,
            IContainer other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            var otherPosition = other.Owner.Transform.MapPosition;

            return ExamineSystemShared.InRangeUnOccluded(origin, otherPosition, range, predicate,
                ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this MapCoordinates origin,
            EntityCoordinates other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true,
            IEntityManager? entityManager = null)
        {
            entityManager ??= IoCManager.Resolve<IEntityManager>();

            var otherPosition = other.ToMap(entityManager);

            return ExamineSystemShared.InRangeUnOccluded(origin, otherPosition, range, predicate,
                ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this MapCoordinates origin,
            MapCoordinates other,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            return ExamineSystemShared.InRangeUnOccluded(origin, other, range, predicate,
                ignoreInsideBlocker);
        }
        #endregion

        #region EventArgs
        public static bool InRangeUnOccluded(
            this ITargetedInteractEventArgs args,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            return ExamineSystemShared.InRangeUnOccluded(args, range, predicate,
                ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this DragDropEvent args,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            return ExamineSystemShared.InRangeUnOccluded(args, range, predicate,
                ignoreInsideBlocker);
        }

        public static bool InRangeUnOccluded(
            this AfterInteractEventArgs args,
            float range = InteractionRange,
            Ignored? predicate = null,
            bool ignoreInsideBlocker = true)
        {
            return ExamineSystemShared.InRangeUnOccluded(args, range, predicate,
                ignoreInsideBlocker);
        }
        #endregion
    }
}
