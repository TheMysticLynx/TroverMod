using System.Threading;
using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    public class TrashMover : SteppedCellProcessor
    {
        public TrashMover(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "Trover";
        public override int CellType => 10;
        public override string CellSpriteIndex => "Trover";
        public override bool OnReplaced(BasicCell basicCell, BasicCell replacingCell)
        {
            return true;
        }

        public override bool TryPush(BasicCell cell, Direction direction, int force)
        {
            if (direction == cell.Transform.Direction)
                force++;
            else if (direction.Axis == cell.Transform.Direction.Axis)
                force--;

            if (force <= 0)
                return false;

            var target = cell.Transform.Position + direction.AsVector2Int;
            if (!_cellGrid.InBounds(target))
                return false;

            cell.Move(target);
            return true;
        }

        public override void OnCellInit(ref BasicCell cell)
        {

        }

        public override void Step(CancellationToken ct)
        {
            foreach (var cell in GetOrderedCellEnumerable())
            {
                if (ct.IsCancellationRequested)
                    return;

                var target = cell.Transform.Position + cell.Transform.Direction.AsVector2Int;
                if (!_cellGrid.InBounds(target))
                    continue;
                var targetCell = _cellGrid.GetCell(target);

                if (targetCell is not null)
                {
                    _cellGrid.RemoveCell(targetCell.Value);
                }

                _cellGrid.PushCell(cell, cell.Transform.Direction, 0);
            }
        }

        public override void Clear()
        {

        }
    }
}