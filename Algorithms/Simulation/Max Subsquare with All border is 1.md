# Max Subsquare with All 1 Borders

给定一个宽高为n的0/1的二值方阵，计算矩阵中满足四条边都是1的最大正方形的面积。

**Example:**

```
Given mat = [	1, 1, 1, 1, 0,
			 	1, 0, 1, 1, 1,
			 	1, 0, 0, 1, 1,
			 	1, 1, 0, 1, 1,
			 	1, 1, 1, 1, 1,
			 	1, 0, 1, 1, 1	]
其中最大的矩阵为左上角边长为4的正方形。
```



## Solution 1: Brute force

### Idea:

与[Leetcode221.Maximal Square](<https://leetcode.com/problems/maximal-square/>)中求内部全为1的最大正方形]的题目不同，该题目看似要求变得更宽松，但是不能用类似的动归思路，目前只想到偏暴力的解决方法。

首先记算每个点向右和向下两个方向最长的可行边长，之后进行逐点遍历，遍历时遍历每个可能的边长，最后取最大值。

Given an array of integers, return **indices** of the two numbers such that they add up to a specific target.

You may assume that each input would have **_exactly_** one solution, and you may not use the _same_ element twice.

### Code:

```c++
#include <iostream>
#include <math.h>
#define MAXN 10000
using namespace std;
int arr[MAXN][MAXN];
// 分别记录向右和向下的最大边长
int dpx[MAXN][MAXN], dpy[MAXN][MAXN];
int main()
{
    int n;
    int ans = 0;
    cin>>n;
    // 读取输入方阵
    for(int i=0; i<n; i++) {
        for(int j=0; j<n; j++) {
            cin>>arr[i][j];
        }
    }

    // 初始化dpx
    for(int i=0; i<n; i++) dpx[i][n-1] = arr[i][n-1];
    // 采用动归的方式对dpx赋值，注意是从右向左遍历
    for(int i=0; i<n; i++) {
        for(int j=n-2; j>=0; j--) {
            if (arr[i][j] == 0) dpx[i][j] = 0;
            else dpx[i][j] = dpx[i][j+1] + 1;
        }
    }

    // 初始化dpy
    for(int i=0; i<n; i++) dpy[n-1][i] = arr[n-1][i];
    // 采用动归的方式对dpy赋值，从下往上
    for(int i=n-2; i>=0; i--) {
        for(int j=0; j<n; j++) {
            if (arr[i][j] == 0) dpy[i][j] = 0;
            else dpy[i][j] = dpy[i+1][j] + 1;
        }
    }


    // 对个点进行遍历
    for(int i=0; i<n; i++) {
        for(int j=0; j<n; j++) {
            // 枚举所有可能的长度
            int length = min(dpx[i][j], dpy[i][j]);
            for(int k = length; k>1; k--) {
                // 如果该长度对应的其余两个点也满足条件
                if(dpx[i+k-1][j]>=k && dpy[i][j+k-1] >= k) {
                    // 则更新ans
                    ans = max(ans, k);
                    break;
                }
            }
        }
    }
    cout<<ans<<endl;
    return 0;
}

```

### Rethink:

- 这里用到了简单的动归，即求类似于最长递增子序列的方法
- 判断矩形是否满足条件时，主要根据四条边，因为遍历length时至少已经有两条边满足了条件，只需要再考察剩下的两条边即可。

## Solution 1: Brute force

### Idea: 

使用两个变量进行遍历，直到满足条件为止。

### Code: 

```c++
class Solution {
public:
    vector<int> twoSum(vector<int>& nums, int target) {
        vector<int> ret;
        for(int i=0; i<nums.size()-1; i++) { // 只到size()-1即可 
            for(int j=i+1; j<nums.size(); j++) { // 从i+1开始即可
                if(nums[i] + nums[j] == target) {
                    ret.push_back(i); ret.push_back(j); return ret;
                    // 必须使用push_back()
                 }
            }
        }
        return ret; // 由于函数必须有返回值，所以这里必须加上
                    // 也可以使用 return {}; 来返回空的vector
    }
};
```

### Rethinking:

- 对vector赋值的两种情况：

  - 动态大小的_vector_要使用_push_back()_赋值，而不能直接通过下标赋值

  - 如果希望通过下标赋值，需要在申请时指定_vector_的大小：

    ```c++
    vector<int> vec(100) // 声明一个长度为100的vector
    ```

- 可以使用：

  ```c++
   return {}; 
  ```

  来返回一个空_vector_。同理，也可以使用_{1, 100}_来表示一个长度为2的_vector_，但这种使用类似_list_的结构来表示_vector_的方法，只有在c++11中才可以使用。

### Complexity Analysis: 

- Time complexity : $O(n^2)$. For each element, we try to find its complement by looping through the rest of array which takes $O(n)$ time. Therefore, the time complexity is $O(n^2)$.
- Space complexity : $O(1)$. 

### Details:

> Runtime: 148 ms, faster than 27.61% of C++ online submissions for Two Sum.
>
> Memory Usage: 9.2 MB, less than 99.94% of C++ online submissions for Two Sum.