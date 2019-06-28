# [1\. Two Sum](https://leetcode.com/problems/two-sum/)

Difficulty: **Easy**

Given an array of integers, return **indices** of the two numbers such that they add up to a specific target.

You may assume that each input would have **_exactly_** one solution, and you may not use the _same_ element twice.

**Example:**

```
Given nums = [2, 7, 11, 15], target = 9,

Because nums[0] + nums[1] = 2 + 7 = 9,
return [0, 1].
```



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

  - 动态大小的 _vector_ 要使用 _push_back()_ 赋值，而不能直接通过下标赋值

  - 如果希望通过下标赋值，需要在申请时指定 _vector_ 的大小：

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