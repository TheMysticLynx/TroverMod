using System.Threading;
using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    public class MoverCellProcessor : SteppedCellProcessor
    {
        public MoverCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "Mover";
        public override int CellType => 1;
        public override string CellSpriteIndex => "Mover";

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

        public override void Step(CancellationToken ct)
        {
            foreach (var cell in GetOrderedCellEnumerable())
            {
                if (ct.IsCancellationRequested)
                    return;
                _cellGrid.PushCell(cell, cell.Transform.Direction, 0);
            }
        }

        public override void Clear()
        {

        }
    }
}