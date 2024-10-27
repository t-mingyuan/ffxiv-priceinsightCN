using System;
using ImGuiNET;

namespace PriceInsight;

internal class ConfigUI(PriceInsightPlugin plugin) : IDisposable {
    private bool settingsVisible = false;

    public bool SettingsVisible {
        get => settingsVisible;
        set => settingsVisible = value;
    }

    public void Dispose() {
    }

    public void Draw() {
        if (!SettingsVisible) {
            return;
        }

        var conf = plugin.Configuration;
        if (ImGui.Begin("Price Insight 设置", ref settingsVisible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.AlwaysAutoResize)) {
            var configValue = conf.RefreshWithAlt;
            if (ImGui.Checkbox("点击 Alt 刷新价格", ref configValue)) {
                conf.RefreshWithAlt = configValue;
                conf.Save();
            }

            configValue = conf.PrefetchInventory;
            if (ImGui.Checkbox("预载入库存物品的价格", ref configValue)) {
                conf.PrefetchInventory = configValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("登录时预载入背包、陆行鸟鞍袋和雇员中所有物品的价格。");

            configValue = conf.UseCurrentWorld;
            if (ImGui.Checkbox("将当前所在的服务器视为原始服务器", ref configValue)) {
                conf.UseCurrentWorld = configValue;
                conf.Save();
                plugin.ClearCache();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("您所在的当前服务器将被视为您的\"原始服务器\"。\n这会帮助您在跨服旅行时查看当前服务器的价格。");

            ImGui.Separator();
            ImGui.PushID(0);

            ImGui.Text("显示以下范围内最便宜的价格:");

            configValue = conf.ShowRegion;
            if (ImGui.Checkbox("中国区", ref configValue)) {
                conf.ShowRegion = configValue;
                conf.Save();
            }
            TooltipRegion();

            configValue = conf.ShowDatacenter;
            if (ImGui.Checkbox("大区", ref configValue)) {
                conf.ShowDatacenter = configValue;
                conf.Save();
            }

            configValue = conf.ShowWorld;
            if (ImGui.Checkbox("原始服务器", ref configValue)) {
                conf.ShowWorld = configValue;
                conf.Save();
            }

            ImGui.PopID();
            ImGui.Separator();
            ImGui.PushID(1);

            ImGui.Text("显示以下范围的数据:");

            configValue = conf.ShowMostRecentPurchaseRegion;
            if (ImGui.Checkbox("中国区", ref configValue)) {
                conf.ShowMostRecentPurchaseRegion = configValue;
                conf.Save();
            }
            TooltipRegion();

            configValue = conf.ShowMostRecentPurchase;
            if (ImGui.Checkbox("大区", ref configValue)) {
                conf.ShowMostRecentPurchase = configValue;
                conf.Save();
            }

            configValue = conf.ShowMostRecentPurchaseWorld;
            if (ImGui.Checkbox("原始服务器", ref configValue)) {
                conf.ShowMostRecentPurchaseWorld = configValue;
                conf.Save();
            }

            ImGui.PopID();
            ImGui.Separator();

            var selectValue = conf.ShowDailySaleVelocityIn;
            if (ImGui.Combo("显示每天的销售额", ref selectValue, "不显示\0服务器\0大区\0区域")) {
                conf.ShowDailySaleVelocityIn = selectValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("根据最近4天的销售额显示每天的平均销售额。");

            selectValue = conf.ShowAverageSalePriceIn;
            if (ImGui.Combo("显示平均售价", ref selectValue, "不显示\0服务器\0大区\0区域")) {
                conf.ShowAverageSalePriceIn = selectValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("显示基于最近4天销售额的平均销售价格。");

            configValue = conf.ShowStackSalePrice;
            if (ImGui.Checkbox("显示总计销售价格", ref configValue)) {
                conf.ShowStackSalePrice = configValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("如果以给定的单价出售，显示总计价格（此单价×数量）。");

            configValue = conf.ShowAge;
            if (ImGui.Checkbox("显示数据时间", ref configValue)) {
                conf.ShowAge = configValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("显示价格信息上次刷新的时间。");

            configValue = conf.ShowDatacenterOnCrossWorlds;
            if (ImGui.Checkbox("显示国外服务器的大区", ref configValue)) {
                conf.ShowDatacenterOnCrossWorlds = configValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("当显示整个区域的价格时，显示来自国外大区的数据。");

            configValue = conf.ShowBothNqAndHq;
            if (ImGui.Checkbox("始终显示 NQ 和 HQ 的价格", ref configValue)) {
                conf.ShowBothNqAndHq = configValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("显示物品NQ和HQ的价格。\n当关闭时将只显示当前品质的价格(使用Ctrl在NQ和HQ之间切换)。");
        }

        ImGui.End();
    }

    private static void TooltipRegion() {
        if (ImGui.IsItemHovered())
            ImGui.SetTooltip("包含所有通过超域旅行能到达的大区。");
    }
}
