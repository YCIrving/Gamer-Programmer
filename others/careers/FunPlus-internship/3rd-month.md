# 第三月：

**时间：2019.07.21-2019.08.30**

**目标：准备秋招**

# Day 38-43: 07.21 - 07.26

最近没怎么更新实习的情况，主要是在调背包的bug，因为需要新增一个使用功能，但是新增后绑定出现了很多问题，花了很长时间才调通。

这里记录一下几个点：
- 在View中绑定的回调函数，不能对model本身进行修改，即在View中不能直接修改Model，只能读取，否则会造成回调函数被递归调用，导致爆栈，所以需要谨记MVC的设计模式。
- Lua里table传递的是引用，用`tableA=tableB`赋值的话，相当于`tableA`指向了`tableB`的位置，对`tableA`进行修改，同样会引起`tableB`的变化。
- 如果tableA绑定了一个viewA，tableB绑定了一个viewB，另外假设我们有一个temp，首先我们让其指向tableA，然后令`temp=tableB`，为了防止同一个view被绑定到两个table上，绑定机制会进行一个检查，即tableA绑定的view与即将tableB的是否相同，如果相同就删去tableA的view。所以解决这个问题的方式就是一定要将各自的view和各自的grid进行绑定。
    ```Lua
    function BagView:Init()
        -- 初始化时创建不同的view，防止在给currentSelectedItemList赋值时将名字相同的view删掉
        self._resourcesView = {}
        self._speedupView = {}
        self._boostsView = {}
        self._othersView = {}
        ...

    -- 绑定时选择各自的view进行绑定
    function BagView:BindResourcesItemList()
        Log("BagView:BindResourcesItemList()")

        XT.bind(self._resourcesView, D.BagModel.ResourcesItemList, function (m, v)
            Log("Bind Resources Item List")
            XT.BindGrid(self, self._resourcesGridView, D.BagModel.ResourcesItemList, VT.BagItemView.Bind)
        end)
    end

    function BagView:BindSpeedupItemList()
        Log("BagView:BindSpeedupItemList()")

        XT.bind(self._speedupView, D.BagModel.SpeedupItemList, function (m, v)
            XT.BindGrid(self, self._speedupGridView, D.BagModel.SpeedupItemList, VT.BagItemView.Bind)
        end)
    end

    function BagView:BindBoostsItemList()
        Log("BagView:BindBoostsItemList()")

        XT.bind(self._boostsView, D.BagModel.BoostsItemList, function (m, v)
            XT.BindGrid(self, self._boostsGridView, D.BagModel.BoostsItemList, VT.BagItemView.Bind)
        end)
    end

    function BagView:BindOthersItemList()
        Log("BagView:BindOthersItemList()")

        XT.bind(self._othersView, D.BagModel.OthersItemList, function (m, v)
            XT.BindGrid(self, self._othersGridView, D.BagModel.OthersItemList, VT.BagItemView.Bind)
        end)
    end
    ```

- 上面的问题修复好以后，又发现了新的问题，就是不能对列表进行排序，因为排序后列表中元素的顺序被打乱了，但没有重新绑定，这时就会出现grid中cell的错乱，这时只要强制让grid和view重新绑定一次即可。
```lua
-- 定义(bindable.lua)
NotifyChanged = function(self)
    local __m_bind = self.__m_bind
    if(__m_bind ~= nil) then
        for _, func in pairs(self.__m_bind) do
            func(self, nil)
        end
    end
end

...

-- 排序(BagView.lua)
table.sort(D.BagModel.ResourcesItemList.__data, BagCtrl.OnSortItemList)
table.sort(D.BagModel.SpeedupItemList.__data, BagCtrl.OnSortItemList)
table.sort(D.BagModel.BoostsItemList.__data, BagCtrl.OnSortItemList)
table.sort(D.BagModel.OthersItemList.__data, BagCtrl.OnSortItemList)

-- 设置选择项（非初始化调用时执行）
if(D.BagModel.NotInit) then
    D.BagModel.ResourcesItemList:NotifyChanged()
    D.BagModel.SpeedupItemList:NotifyChanged()
    D.BagModel.BoostsItemList:NotifyChanged()
    D.BagModel.OthersItemList:NotifyChanged()
    BagCtrl.RestoreCurrentSelection()
end

...

-- ui/BingGrid.lua
local bind = require("core.bind")
local unbind = require("core.unbind")

local function BindGrid(view, grid, list, binder)

    grid.UpdateViewItem = function(index, v)
        local m =  list[index]
        -- 修正绑定的引用，防止多个view被绑定到同一个数据上
        if (v.CellIndex ~= index) or (v.CellModel ~= m) then
            unbind(v)
        end
        v.CellIndex = index
        v.CellModel = m
        binder(m, v)
    end

    if list.__m_bind ~= nil then
        bind(view, list, function (l)
            grid:Refresh(#list.__data, false, true)
        end)
    else
        grid:Refresh(#list, false, true)
    end
end

return BindGrid
```

# Day 44-45: 07.27, 07.29
- IntelliJ中按下alt键然后框选代码可以同时选中多行，然后同时编辑（按住鼠标中键拖动也可以，但记得是框选）
