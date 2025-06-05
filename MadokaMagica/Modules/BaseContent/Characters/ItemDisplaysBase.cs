﻿using RoR2;
using System.Collections.Generic;

namespace MadokaMagica.Modules.Characters
{
    public abstract class ItemDisplaysBase
    {
        public void SetItemDisplays(ItemDisplayRuleSet itemDisplayRuleSet)
        {
            var itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();

            ItemDisplays.LazyInit();

            SetItemDisplayRules(itemDisplayRules);

            itemDisplayRuleSet.keyAssetRuleGroups = itemDisplayRules.ToArray();

            ItemDisplays.DisposeWhenDone();
        }

        protected abstract void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules);
    }
}
