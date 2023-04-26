using System.Threading;
using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    public class GeneratorCellProcessor: SteppedCellProcessor
    {
        public GeneratorCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "Generator";
        public override int CellType => 2;
        public override string CellSpriteIndex => "Generator";

        public override bool TryPush(BasicCell cell, Direction direction, int force)
        {
            if (force <= 0)
                return false;

            var target = cell.Transform.Position + direction.AsVector2Int;
            if (!_cellGrid.InBounds(target))
                return false;
            var targetCell = _cellGrid.GetCell(target);

            if (targetCell is null)
            {
                cell.Move(target);
                return true;
            }

            if (!_cellGrid.PushCell(targetCell.Value, direction, force))
                return false;


            cell.Move(target);
            return true;
        }

        public override void OnCellInit(ref BasicCell cell)
        {
        }

        public override bool OnReplaced(BasicCell basicCell, BasicCell replacingCell)
        {
            return true;
        }

        public override void Step(CancellationToken ct)
        {
            foreach (var cell in GetOrderedCellEnumerable())
            {
                if(ct.IsCancellationRequested)
                    return;
                var copyCell = _cellGrid.GetCell(cell.Transform.Position - cell.Transform.Direction.AsVector2Int);
                if (copyCell is null)
                    continue;

                var targetPos = cell.Transform.Position + cell.Transform.Direction.AsVector2Int;

                if (!_cellGrid.InBounds(targetPos))
                    continue;

                var targetCell = _cellGrid.GetCell(targetPos);
                if(targetCell != null)
                    if (!_cellGrid.PushCell(targetCell.Value, cell.Transform.Direction, 1))
                        continue;

                var newCellTransform = cell.Transform;
                newCellTransform.Direction = copyCell.Value.Transform.Direction;
                var prevTransform = newCellTransform;
                prevTransform.ZIndex = 1;
                var newCell = _cellGrid.AddCell(targetPos, copyCell.Value.Transform.Direction, copyCell.Value.Instance.Type, prevTransform);
            }
        }

        public override void Clear()
        {

        }
    }
}