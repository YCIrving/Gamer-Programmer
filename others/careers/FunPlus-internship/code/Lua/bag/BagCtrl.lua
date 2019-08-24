---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by YCIrving.
--- DateTime: 2019-07-11 18:12
---
---
---@class BagCtrl
local BagCtrl = XT.class()
local BindableList = XT.bindable()

function BagCtrl.InitData()
    D.BagModel.ResourcesItemList = BindableList()
    D.BagModel.SpeedupItemList = BindableList()
    D.BagModel.BoostsItemList = BindableList()
    D.BagModel.OthersItemList = BindableList()
    D.BagModel.AddedItemList = BindableList()

    if D.PlayerData == nil then
        Warn("D.PlayerData is nil")
        return
    end

    D.PlayerData.Bag:OnChanged(function (bag)

        print("D.PlayerData.Bag:OnChanged")

--[[        D.BagModel.ResourcesItemList:Clear()
        D.BagModel.SpeedupItemList:Clear()
        D.BagModel.BoostsItemList:Clear()
        D.BagModel.OthersItemList:Clear()]]

        local resourcesItemListSize = 0
        local speedupItemListSize = 0
        local boostsItemListSize = 0
        local othersItemListSize = 0
        for k, v in pairs(bag.Items) do
            local data = MT.BagItemModel()
            data.Count = v
            data.ObjectConfItem = T.ObjectConf[k]
            data.Selected = false
            -- 红点提示有疑问
            data.TipSpot = true

            -- 上线代码(按类型过滤)
            if(data.ObjectConfItem.DisplayType == 1) then
                resourcesItemListSize = resourcesItemListSize + 1
                if(D.BagModel.ResourcesItemList[resourcesItemListSize] ~= nil) then
                    local resourcesItemTemp = D.BagModel.ResourcesItemList[resourcesItemListSize]
                    resourcesItemTemp.Count = data.Count
                    resourcesItemTemp.ObjectConfItem = data.ObjectConfItem
                    resourcesItemTemp.Selected = data.Selected
                    resourcesItemTemp.TipSpot = data.TipSpot
                else
                    D.BagModel.ResourcesItemList[resourcesItemListSize] = data
                end

            elseif(data.ObjectConfItem.DisplayType == 2) then
                speedupItemListSize = speedupItemListSize + 1
                if(D.BagModel.SpeedupItemList[speedupItemListSize] ~= nil) then
                    local speedupItemTemp = D.BagModel.ResourcesItemList[speedupItemListSize]
                    speedupItemTemp.Count = data.Count
                    speedupItemTemp.ObjectConfItem = data.ObjectConfItem
                    speedupItemTemp.Selected = data.Selected
                    speedupItemTemp.TipSpot = data.TipSpot
                else
                    D.BagModel.SpeedupItemList[speedupItemListSize] = data
                end

            elseif(data.ObjectConfItem.DisplayType == 3) then
                boostsItemListSize = boostsItemListSize + 1
                if(D.BagModel.BoostsItemList[boostsItemListSize] ~= nil) then
                    local boostsItemTemp = D.BagModel.BoostsItemList[boostsItemListSize]
                    --boostsItemTemp.BeginModification()
                    boostsItemTemp.Count = data.Count
                    boostsItemTemp.ObjectConfItem = data.ObjectConfItem
                    boostsItemTemp.Selected = data.Selected
                    boostsItemTemp.TipSpot = data.TipSpot
                    -- boostsItemTemp.EndModification()
                else
                    D.BagModel.BoostsItemList[boostsItemListSize] = data
                end
            elseif(data.ObjectConfItem.DisplayType == 4) then
                othersItemListSize = othersItemListSize + 1
                if(D.BagModel.OthersItemList[othersItemListSize] ~= nil) then
                    local othersItemTemp = D.BagModel.OthersItemList[othersItemListSize]
                    othersItemTemp.Count = data.Count
                    othersItemTemp.ObjectConfItem = data.ObjectConfItem
                    othersItemTemp.Selected = data.Selected
                    othersItemTemp.TipSpot = data.TipSpot
                else
                    D.BagModel.OthersItemList[othersItemListSize] = data
                end
            end
        end

        -- 检查列表
        while(resourcesItemListSize<D.BagModel.ResourcesItemList:Count())
        do
            D.BagModel.ResourcesItemList:RemoveAt(D.BagModel.ResourcesItemList:Count())
        end
        while(speedupItemListSize<D.BagModel.SpeedupItemList:Count())
        do
            D.BagModel.SpeedupItemList:RemoveAt(D.BagModel.SpeedupItemList:Count())
        end
        while(boostsItemListSize<D.BagModel.BoostsItemList:Count())
        do
            D.BagModel.BoostsItemList:RemoveAt(D.BagModel.BoostsItemList:Count())
        end
        while(othersItemListSize<D.BagModel.OthersItemList:Count())
        do
            D.BagModel.OthersItemList:RemoveAt(D.BagModel.OthersItemList:Count())
        end

        -- 排序
        table.sort(D.BagModel.ResourcesItemList.__data, BagCtrl.OnSortItemList)
        table.sort(D.BagModel.SpeedupItemList.__data, BagCtrl.OnSortItemList)
        table.sort(D.BagModel.BoostsItemList.__data, BagCtrl.OnSortItemList)
        table.sort(D.BagModel.OthersItemList.__data, BagCtrl.OnSortItemList)

        -- D.BagModel.ResourcesItemList:NotifyChanged()

        -- 设置选择项（非初始化调用时执行）
        if(D.BagModel.NotInit) then
            D.BagModel.ResourcesItemList:NotifyChanged()
            D.BagModel.SpeedupItemList:NotifyChanged()
            D.BagModel.BoostsItemList:NotifyChanged()
            D.BagModel.OthersItemList:NotifyChanged()
            BagCtrl.RestoreCurrentSelection()
        end

        D.BagModel.NotInit = true
    end)

    BagCtrl.InitListSelection()

    -- D.BagModel.SelectedItem.NumUse = 1

end

function BagCtrl.OnBtnExit()
    BagCtrl.OnSwitchScene()
end

function BagCtrl.OnBtnMinus()
    Log("Button Minus Clicked.")
    D.BagModel.SelectedItem.NumUse = BagCtrl.CheckCurrentNumUse(D.BagModel.SelectedItem.NumUse - 1)
end

function BagCtrl.OnLongPressBtnMinus()
    Log("Button Minus LongPressed.")
    D.BagModel.SelectedItem.NumUse = BagCtrl.CheckCurrentNumUse(D.BagModel.SelectedItem.NumUse - 1)
end

function BagCtrl.OnBtnPlus()
    Log("Button Plus Clicked.")
    D.BagModel.SelectedItem.NumUse = BagCtrl.CheckCurrentNumUse(D.BagModel.SelectedItem.NumUse + 1)
    -- BagCtrl.CheckCurrentNumUse()
end

function BagCtrl.OnLongPressBtnPlus()
    Log("Button Plus LongPressed.")
    D.BagModel.SelectedItem.NumUse = BagCtrl.CheckCurrentNumUse(D.BagModel.SelectedItem.NumUse + 1)
end


function BagCtrl.OnConfirm()
    Log("BagCtrl.OnConfirm")
end

function BagCtrl.OnCancel()
    Log("BagCtrl.OnCancel")
end

function BagCtrl.OnBtnUse()
    -- 注意：后期还需要实现"每次只能使用一个"的功能
    Log("Button Use Clicked.")
    if(D.BagModel.SelectedItem.NumUse == D.BagModel.SelectedItem.CurrentSelectedItem.Count) then
        D.BagModel.SelectedItem.UseUp = true
    else
        D.BagModel.SelectedItem.UseUp = false
    end
    D.BagModel.SelectedItem.CurrentSelectedItem.Selected = false

    local addAttribute = D.BagModel.SelectedItem.CurrentSelectedItem.ObjectConfItem.AddAttribute

    -- 严格按照需求文档实现
    -- 使用add_object类物品，string ID = 1412
    if addAttribute == "add_object" then
        S:UseItem(D.BagModel.SelectedItem.CurrentSelectedItem.ObjectConfItem.ID, D.BagModel.SelectedItem.NumUse, function (placeholder, err)
            -- 对服务器回复的物品增加消息进行处理
            if err ~= nil then
                error(err)
            end

            local messageString = "You get "
            for itemID, itemNum in pairs(placeholder.AddedItems) do
                local itemName = T.ObjectConf[itemID].Name
                messageString = messageString..tostring(itemNum).." "..itemName
            end

            -- 提示信息
            XT.UIManager.Instance:Open(PanelName.MESSAGE_HINT_PANEL,function ()
                local data = {
                    Content = messageString
                }
                Event.Brocast(UIEventType.SHOW_MESSAGE_HINT_PANEL, data)
            end)

            -- 还需要补充红点
            print("S:UseItem: " .. tostring(placeholder))

        end)

    -- 使用add_gem或add_equipment类物品，string ID = 1408
    elseif addAttribute == "add_gem" or addAttribute == "add_equipment" then
        S:UseItem(D.BagModel.SelectedItem.CurrentSelectedItem.ObjectConfItem.ID, D.BagModel.SelectedItem.NumUse, function (placeholder, err)
            -- 对服务器回复的物品增加消息进行处理
            if err ~= nil then
                error(err)
            end

            local messageString = "You have used "..D.BagModel.SelectedItem.NumUse .. " " .. D.BagModel.SelectedItem.CurrentSelectedItem.ObjectConfItem.Name

            -- 提示信息
            XT.UIManager.Instance:Open(PanelName.MESSAGE_HINT_PANEL,function ()
                local data = {
                    Content = messageString
                }
                Event.Brocast(UIEventType.SHOW_MESSAGE_HINT_PANEL, data)
            end)
            print("S:UseItem: " .. tostring(placeholder))

        end)

    -- 使用buff的提示信息
    elseif addAttribute == "add_buff" then
--[[        local buffID = D.BagModel.SelectedItem.CurrentSelectedItem.ObjectConfItem.parameters[1]
        local buffGroup = T.BuffConf[buffID].Group
        local buffGroupChang = T.BuffConf[buffID].GroupChang

        -- 获得当前玩家buff，暂未实现
        -- local currentBuffID = D.PlayerData.Buff
        -- local currentBuffGroup = T.BuffConf[currentBuffID].Group
        -- local currentBuffGroupChang = T.BuffConf[currentBuffID].GroupChang

        -- 判断提示性质
        -- 可以叠加或者不在同一组
        if (buffGroupChang == 2 or buffGroup ~= currentBuffGroup ) then
            S:UseItem(D.BagModel.SelectedItem.CurrentSelectedItem.ObjectConfItem.ID, D.BagModel.SelectedItem.NumUse, function (placeholder, err)
                -- 对服务器回复的物品增加消息进行处理
                if err ~= nil then
                    error(err)
                end

                local messageString = "You have used "..D.BagModel.SelectedItem.NumUse .. " " .. D.BagModel.SelectedItem.CurrentSelectedItem.ObjectConfItem.Name

                -- 提示信息
                XT.UIManager.Instance:Open("MessageHint",function ()
                    local data = {
                        Content = messageString
                    }
                    Event.Brocast(UIEventType.SHOW_MESSAGE_HINT_PANEL, data)
                end)
                print("S:UseItem: " .. tostring(placeholder))

            end)
        -- 如果不能叠加，则需要提示用户确认
        else
            XT.UIManager.Instance:Open("messagebox",function ()
                local data = {
                    -- string 1409
                    -- This string makes me laugh.
                    Content = "Using the current items, will replace the original buff, whether or not to use the item?",
                        OnConfirm = BagCtrl.OnConfirmUsingBuff, OnCancel = BagCtrl.OnCancelUsingBuff
                }
                Event.Brocast(UIEventType.SHOW_MESSAGE_BOX_PANEL, data)
            end)
            -- 在OnConfirmUsingBuff()中写使用函数
        end]]

        -- 具体实现使用buff时，删掉下面的代码
        XT.UIManager.Instance:Open(PanelName.MESSAGE_HINT_PANEL,function ()
            local data = {
                Content = "Not implemented error!"
            }
            Event.Brocast(UIEventType.SHOW_MESSAGE_HINT_PANEL, data)
        end)

    -- 迁城道具，目前仅实现跳转功能
    elseif addAttribute == "go_map" then
        XT.UIManager.Instance:Open(PanelName.MAIN_CITY_PANEL,function ()
            XT.UIManager.Instance:OnDestroy(PanelName.BAG_PANEL)
        end)

    -- 开宝箱的提示信息
    elseif addAttribute == "add_reward" then
        S:UseItem(D.BagModel.SelectedItem.CurrentSelectedItem.ObjectConfItem.ID, D.BagModel.SelectedItem.NumUse, function (placeholder, err)
            -- 对服务器回复的物品增加消息进行处理
            if err ~= nil then
                error(err)
            end

            -- 清空AddedItemList
            D.BagModel.AddedItemList:Clear()
            local addedItemListSize = 0
            for itemID, itemNum in pairs(placeholder.AddedItems) do
                addedItemListSize = addedItemListSize + 1
                local addItemTemp = MT.ChestItemModel()
                addItemTemp.Count = itemNum
                addItemTemp.ObjectConfItem = T.ObjectConf[itemID]
                addItemTemp.Selected = false
                addItemTemp.TipSpot = false
                D.BagModel.AddedItemList[addedItemListSize] = addItemTemp
            end

            if(addedItemListSize ~= 0) then
                XT.UIManager.Instance:Open(PanelName.CHEST_PANEL,function ()
                    local data = {

                    }
                    Event.Brocast(UIEventType.SHOW_UI_OPEN_CHEST_PANEL, data)
                end)
            end
            -- 提示信息

            print("S:UseItem: " .. tostring(placeholder))
        end)
    else

    end
    Log("Used ".. D.BagModel.SelectedItem.NumUse .." ".. D.BagModel.SelectedItem.CurrentSelectedItem.ObjectConfItem.Name .. ".")
end

function BagCtrl.OnInputNum(val)
    D.BagModel.SelectedItem.NumUse = BagCtrl.CheckCurrentNumUse(tonumber(val))
end

function BagCtrl.CheckCurrentNumUse(val)
    if(val < 1 or D.BagModel.SelectedItem.CurrentSelectedItem.ObjectConfItem.UseType == 1) then
        val = 1
    end
    if(val > D.BagModel.SelectedItem.CurrentSelectedItem.Count) then
        val =  D.BagModel.SelectedItem.CurrentSelectedItem.Count
    end
    return val
end

function BagCtrl.OnBtnMax()
    Log("Button Max Clicked.")
    D.BagModel.SelectedItem.NumUse = D.BagModel.SelectedItem.CurrentSelectedItem.Count
end

function BagCtrl.OnSwitchScene()
    XT.UIManager.Instance:Open(PanelName.MAIN_CITY_PANEL,function ()

        XT.UIManager.Instance:OnDestroy(PanelName.BAG_PANEL)
    end)
end


function BagCtrl.InitListSelection()
    if (D.BagModel.ResourcesItemList:Count()>0) then
        D.BagModel.ResourcesSelectedIndex = 1
    end
    if (D.BagModel.SpeedupItemList:Count()>0) then
        D.BagModel.SpeedupSelectedIndex = 1
    end
    if (D.BagModel.BoostsItemList:Count()>0) then
        D.BagModel.BoostsSelectedIndex = 1
    end
    if (D.BagModel.OthersItemList:Count()>0) then
        D.BagModel.OthersSelectedIndex = 1
    end
    -- BagCtrl.SetCurrentSelection(D.BagModel.ResourcesItemList, D.BagModel.ResourcesSelectedIndex)

end

-- 切换tab时执行
function BagCtrl.SetCurrentSelection(TargetItemList, TargetIndex)

    -- 赋值新的ItemList
    D.BagModel.SelectedItemList.CurrentSelectedItemList = TargetItemList

    -- 检查ItemList是否为空
    local CurrentSelectedItemListEmpty = true
    if(D.BagModel.SelectedItemList.CurrentSelectedItemList:Count() ~= 0) then
        CurrentSelectedItemListEmpty = false
    end
    D.BagModel.SelectedItemList.CurrentSelectedItemListEmpty = CurrentSelectedItemListEmpty


    -- 将原始selection取消掉
    if(D.BagModel.SelectedItem.CurrentSelectedItem ~= nil) then
        D.BagModel.SelectedItem.CurrentSelectedItem.Selected = false
    end

    -- 赋值新的Item
    if (TargetIndex <= TargetItemList:Count() and TargetIndex>0) then
        D.BagModel.SelectedItem.CurrentSelectedItem = TargetItemList[TargetIndex]
        D.BagModel.SelectedItem.CurrentSelectedItem.Selected = true
        D.BagModel.SelectedItem.CurrentSelectedIndex = TargetIndex
    else
        D.BagModel.SelectedItem.CurrentSelectedItem = nil
        D.BagModel.SelectedItem.CurrentSelectedIndex = 0
    end
end

function BagCtrl.UpdateItemListSelectedIndex()
    if(D.BagModel.SelectedItemList.CurrentSelectedItemList == D.BagModel.ResourcesItemList) then
        D.BagModel.ResourcesSelectedIndex = D.BagModel.SelectedItem.CurrentSelectedIndex
    elseif(D.BagModel.SelectedItemList.CurrentSelectedItemList == D.BagModel.SpeedupItemList) then
        D.BagModel.SpeedupSelectedIndex = D.BagModel.SelectedItem.CurrentSelectedIndex
    elseif(D.BagModel.SelectedItemList.CurrentSelectedItemList == D.BagModel.BoostsItemList) then
        D.BagModel.BoostsSelectedIndex = D.BagModel.SelectedItem.CurrentSelectedIndex
    elseif(D.BagModel.SelectedItemList.CurrentSelectedItemList == D.BagModel.OthersItemList) then
        D.BagModel.OthersSelectedIndex = D.BagModel.SelectedItem.CurrentSelectedIndex
    end
end

-- 使用物品时执行
function BagCtrl.RestoreCurrentSelection()

    if (D.BagModel.SelectedItemList.CurrentSelectedItemList == nil or D.BagModel.SelectedItemList.CurrentSelectedItemList:Count() == 0) then
        D.BagModel.SelectedItem.CurrentSelectedIndex = 0
        D.BagModel.SelectedItemList.CurrentSelectedItemListEmpty = true
    elseif (D.BagModel.SelectedItemList.CurrentSelectedItemList:Count()< D.BagModel.SelectedItem.CurrentSelectedIndex) then
        D.BagModel.SelectedItem.CurrentSelectedIndex = D.BagModel.SelectedItemList.CurrentSelectedItemList:Count()
    end
    BagCtrl.UpdateItemListSelectedIndex()
    -- 赋值新的Item
    if (D.BagModel.SelectedItem.CurrentSelectedIndex>0) then
        D.BagModel.SelectedItem.CurrentSelectedItem = D.BagModel.SelectedItemList.CurrentSelectedItemList[D.BagModel.SelectedItem.CurrentSelectedIndex]
        D.BagModel.SelectedItem.CurrentSelectedItem.Selected = true
        D.BagModel.SelectedItem.CurrentSelectedItem.TipSpot = false
        D.BagModel.SelectedItem.NumUse = BagCtrl.CheckCurrentNumUse(D.BagModel.SelectedItem.NumUse)
    else
        D.BagModel.SelectedItem.CurrentSelectedItem = nil
        D.BagModel.SelectedItem.CurrentSelectedIndex = 0
    end
end

function BagCtrl.OnBtnResourcesTab(val)
    Log("Resources:"..tostring(val))
    --[[    D.BagModel.ChooseResource = val]]
    if(val) then
        BagCtrl.SetCurrentSelection(D.BagModel.ResourcesItemList, D.BagModel.ResourcesSelectedIndex)
    end

end

function BagCtrl.OnBtnSpeedupTab(val)
    Log("Speedup:"..tostring(val))
    --[[    D.BagModel.ChooseSpeedup = val]]
    if(val) then
        BagCtrl.SetCurrentSelection(D.BagModel.SpeedupItemList, D.BagModel.SpeedupSelectedIndex)
    end

end

function BagCtrl.OnBtnBoostsTab(val)
    Log("Boosts:"..tostring(val))
    --[[    D.BagModel.ChooseBoots = val]]
    if(val) then
        BagCtrl.SetCurrentSelection(D.BagModel.BoostsItemList, D.BagModel.BoostsSelectedIndex)
    end

end

function BagCtrl.OnBtnOthersTab(val)
    Log("Others:"..tostring(val))
    --[[    D.BagModel.ChooseOthers = val]]
    if(val) then
        BagCtrl.SetCurrentSelection(D.BagModel.OthersItemList, D.BagModel.OthersSelectedIndex)
    end

end

function BagCtrl.OnSortItemList(a, b)
    -- 如果物品不能使用，则排在可使用的后面
    if (a.ObjectConfItem.UseType ~= b.ObjectConfItem.UseType and
            (a.ObjectConfItem.UseType==0 or b.ObjectConfItem.UseType==0)) then
        -- 如果a可使用，返回true，a在前面
        -- 否则返回false，b在前面
        return a.ObjectConfItem.UseType>0
    else
        return a.ObjectConfItem.ID < b.ObjectConfItem.ID
    end
end

return BagCtrl