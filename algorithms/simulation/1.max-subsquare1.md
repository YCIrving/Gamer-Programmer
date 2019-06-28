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

### Complexity Analysis: 

- Time complexity : $O(2n^2 + n^3)$. 初期的动归计算需要两次数量级为$O(n^2)$的遍历，之后对每个点进行遍历，遍历需要枚举所有长度，所以复杂度为$O(n^3)$。
- Space complexity : $O(2n^2)$. 
