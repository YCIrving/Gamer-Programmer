# Top K 问题
定义：在n个数中，找到前k大个数字

## Solution 1 : 排序
### Idea: 
对这n个数进行排序，然后选择前K个数字即可
### Complexity Analysis:
与使用的排序算法有关，如快排的复杂度为$O(nlogn)$

## Solution 2 : 快速选择
### Idea: 
任意选择一个轴，然后将比轴大的放在轴左边，比轴小的放在轴右边。如果左边的数大于k，则继续在左边划分，否则在右边划分。

### Code:
```c++
class Solution {
public:
    int findKthLargest(vector<int>& nums, int k) {
        int left = 0, right = nums.size() - 1, kth;
        while (true) {
            int idx = partition(nums, left, right);
            if (idx == k - 1) {
                kth = nums[idx];
                break;
            }
            if (idx < k - 1) {
                left = idx + 1;
            } else {
                right = idx - 1;
            }
        }
        return kth;
    }
private:
    // 同样可以用在快排中
    int partition(vector<int>& nums, int left, int right) {
        int pivot = nums[left], l = left + 1, r = right;
        while (l <= r) {
            // 注意这里是r--
            if (nums[l] < pivot && nums[r] > pivot) {
                swap(nums[l++], nums[r--]);
            }
            if (nums[l] >= pivot) {
                l++;
            }
            if (nums[r] <= pivot) {
                r--;
            }
        }
        swap(nums[left], nums[r]);
        return r;
    }
};
```

### Complexity Analysis:
时间复杂度为$O(n)$，因为第一次划分需要比较n个数，第二次为$O(n/2)$，渐进下就是$O(n)$，因为与快排不同，不需要遍历两支，只需要选择其中一支遍历。但最坏情况下时间复杂度仍是$O(n^2)$

## Solution 3 : 分布式
### Idea: 
如果数字的数量比较多，内存无法一次性放下所有数字，则可以采用分布式做法，将数字平均划分到多个主机上，每个主机计算Top k，然后将所有的Top k放在一起计算最终的Top k。

## Solution 4 : 堆
### Idea: 
用于内存不足且仅有一台主机的情况，首先用序列的前k个数字建立一个大小为k的小顶堆，然后对后续所有的数字进行比较，如果大于堆顶元素，则将堆顶替换，然后进行调整，否则直接丢弃，因为一定不是前k大的数字。

### Code:
```c++
public class TopK {
    // 父节点
    private int parent(int n)
    {
        return (n-1)/2;
    }
    // 左孩子
    private int left(int n)
    {
        return 2*n+1;
    }
    // 右孩子
    private int right(int n)
    {
        return 2*n+2;
    }

    // 建堆
    private void buildHeap (int k, int[] data)
    {
        // ** M 两层循环，向上调整，所以0不需要调整 **
        for(int i=1; i<k; i++)
        {
            int t = i;
            // ** M 判断条件 **
            while(t!=0 && data[parent(t)]>data[t])
            {
                // swap data[t] and data[parent(t)]
                int temp = data[parent(t)];
                data[parent(t)] = data[t];
                data[t] = temp;
                t = parent(t);
            }
        }
    }

    // 调整堆
    private void adjust(int i, int k, int []data)
    {
        if(data[i]<= data[0]) return;

        // 置换堆顶元素
        int temp = data[i];
        data[i] = data[0];
        data[0] = temp;

        //调整堆顶
        int t = 0;
        // 左孩子需要交换 || 右孩子需要交换
        while(left(t)<k && data[t]> data[left(t)] || right(t)<k && data[t] > data[right(t)])
        
        {
            // 这里特别容易错
            // 首先，如果进到这里，left(t)一定是小于k，且一定有孩子要被交换
            // 其次，我们默认交换左孩子，但如果要交换右孩子，条件为
            // 右孩子下标小于k，并且右孩子小于左孩子
            // ** M 判断条件 **
            if(right(t) <k && data[right(t)]<data[left(t)])
            {
                // swap data[right(t)] and data[t]
                temp = data[right(t)];
                data[right(t)] = data[t];
                data[t] = temp;
                t = right(t);
            }
            // 否则交换左孩子
            else
            {
                // swap data[left(t)] and data[t]
                temp = data[left(t)];
                data[left(t)] = data[t];
                data[t] = temp;
                t = left(t);

            }            
        }
    }

    // 寻找TopK
    public void findTopK(int k, int[] data)
    {
        buildHeap(n, data);
        for(int i=k; i<data.length; i++)
        {
            adjust(i, k, data);
        }
    }

    // 输出结果
    public void print(int[] data)
    {
        for(int i=0; i<k; i++)
        {
            cout<<data[i]<<' ';
        }
    }
}
```
### Rethinking:
本题关键是通过前k个数构建堆和对堆的调整；
构建时，从堆顶开始，一直往后进行调整，每回调整都需要一直从下往上调整到堆顶结束。调整堆时，对于从堆顶开始，一直往下调整，将堆顶元素与左右孩子中更小的那个进行交换，交换后继续往下，直到超过k或者满足堆的条件为止。

一点重要的区别：
- 构建堆时，是对前k个元素都进行调整
- 调整堆时，只对堆顶元素进行调整（`data[0]`）

### Complexity Analysis:
K个数建堆的复杂度为$O(k)$，之后调整一次的复杂度为$O(logk)$，大致执行n次，所以总的时间复杂度为$O(n*logk)$
