using Ferustria.Common.UIs.Elements;
using Terraria.UI;

namespace Ferustria.Common.UIs.States
{
    internal class Weapon_ChargeBars_UIState : UIState
    {
        public CKnife1_Charge_Bar cKnife1_ChargeBar;
        public Rozaline_ChargeBar rozaline_ChargeBar;
        public VoidPruner_ChargeBar voidPruner_ChargeBar;

        public override void OnInitialize()
        {
            cKnife1_ChargeBar = new CKnife1_Charge_Bar();
            Append(cKnife1_ChargeBar);
            rozaline_ChargeBar = new Rozaline_ChargeBar();
            Append(rozaline_ChargeBar);
            voidPruner_ChargeBar = new VoidPruner_ChargeBar();
            Append(voidPruner_ChargeBar);
        }
    }
}
