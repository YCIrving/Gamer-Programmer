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

# Day 46: 08.05
- 对数字显示在屏幕上的转化的实现
    - 对于特别大的数字，如果直接显示在屏幕上会导致空间不够，所以需要一些压缩操作。
    - 操作要求：1)按照不同大小压缩，比如小于1K不压缩，大于等于1K但小于1M时，压缩为后缀为K的数字，大于等于1M时，压缩为后缀为M的数字。2)压缩后不能四舍五入，而要永远舍去多余的小数点。
    - 对于第一个需求，可以采用分位点判断的形式，即首先判断是否需要压缩，如果是，则从大到小依次遍历分位点，如果满足压缩要求则进行压缩
    - 对于第二个需求，可以采用先乘再除的形式，比如要将34567压缩为34.5K，直接除以1000的答案是34.567K，之后再保留一位小数位34.6K，不满足要求。所以我们定义power为10^decimal，decimal为保留的小数点位数，然后我们将压缩后的结果乘以power，再取floor，之后再除以power即可，比如上面得例子中我们的计算过程为
        ```
        power = 10^1 // 因为保留一位小数，所以是10的1次方
        34567/1000 = 34.567
        34.567*power = 345.67 // 先乘power
        floor(345.67) = 345 // 之后下取整，即floor
        345/power = 34.5 // 最后除以power
        // 输出34.5K
        ```
    - 源代码如下:
        ```Lua
        -- 把数字转换为显示在界面上的字符串
        -- num: 待转换的数字
        -- quantileList: 数字分位点列表, 通常为10^3、10^4、10^6等，不同语言可能不同
        -- suffixList: 后缀列表，对应每个分位点的后缀，比如"K"、"万"、"M"等，不足第一个分位点将没有后缀
        -- decimals: 需要保留的小数点位数
        function CommonFunUtil.NumberToStrForShowing(num, quantileList, suffixList, decimals)
            if (num==nil) then return "" end

            if(num<quantileList[1]) then return tostring(num) end

            local quantileListLen = 0
            for id, val in pairs( quantileList ) do
                quantileListLen = quantileListLen + 1
            end

            local suffixListLen = 0
            for id, val in pairs( suffixList ) do
                suffixListLen = suffixListLen + 1
            end

            assert(quantileListLen == suffixListLen, "The lengths of quantileList and suffixList must be equal!")

            local power = 10^decimals
            local fmt = "%."..decimals.."f"

            for i=quantileListLen,1,-1 do
                if((num/quantileList[i])>=1) then
                    return string.format(fmt.."%s", math.floor(num/quantileList[i]*power)/power,suffixList[i])
                end
            end
        end        
        ```
