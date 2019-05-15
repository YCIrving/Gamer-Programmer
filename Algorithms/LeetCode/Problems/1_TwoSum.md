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
使用两个变量进行遍历，直到满足条件为止


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
* Time complexity : $O(n^2)$. For each element, we try to find its complement by looping through the rest of array which takes $O(n)$ time. Therefore, the time complexity is $O(n^2)$.

* Space complexity : $O(1)$. 

### Details:

> Runtime: 148 ms, faster than 27.61% of C++ online submissions for Two Sum.
> 
> Memory Usage: 9.2 MB, less than 99.94% of C++ online submissions for Two Sum.



## Solution 2: Two-pass Hash Table

### Idea: 

使用哈希表，在第一轮循环中，将所有数字填入表中，在第二轮中验证是否存在相加得到target的另一个数字。

### Code: 

```c++
class Solution {
public:
    vector<int> twoSum(vector<int>& nums, int target) {
        unordered_map<int, int> hash_map;
        vector<int> ret(2);
        // 第一轮循环，加入键值
        for(int i=0; i<nums.size(); i++) hash_map[nums[i]] = i;
        // 第二轮循环，验证是否存在
        // 十分重要的一点在于，每个数字只能用一次的判断
        for (int i=0; i<nums.size(); i++) {
            if(hash_map.find(target-nums[i])!=hash_map.end() && hash_map[target-nums[i]] != i) {
                ret[0] = i; ret[1] = hash_map[target-nums[i]];
                return ret;
            }
        }
        return {};
    }
};
```
### Rethinking:

* 对于每个数字只能用一次的判断是本题容易忽视的问题，比如对于测试用例：

  ```
  Input: [3, 2, 4] 6
  Expected Output: {1, 2}
  ```

  而如果不加入判定

  ```c++
  hash_map[target-nums[i]] != i
  ```

  的话，就会导致输出{0, 0}，此时数字3被使用了两次。

- 哈希表的STL实现主要包括两类：

  - _map_: 内部有序，但维护需要花费$O(logn)$的时间

    ```c++
    map<int, int> my_map; //声明一个map
    ```

  - _unordered_map_: 内部无序，增删改查均为$O(1)$时间（最坏为$O(n)$），可以看做是哈希表的官方实现

    ```c++
    // 声明一个哈希表
    unordered_map<int, int> my_unordered_map; 
    ```

  - 二者更多区别可见[GeeksforGeeks](<https://www.geeksforgeeks.org/map-vs-unordered_map-c/>)

- 对哈希表的基本操作包括增删改查和遍历：

  ```c++
  // 增加元素可以直接根据下标，也可以使用insert()函数
  my_map[1] = 100;
  my_map.insert(pair<int, int> (1, 100));
  my_map.insert(make_pair(1, 100));
  
  // 删除元素可以使用erase()函数
  my_map.erase(1);
  
  // 修改可以直接重新赋值
  my_map[1] = 200;
  
  // 查找可以使用find()函数
  if (my_map.find(1) != my_map.end()) // 不等说明找到键值
      cout<<my_map[1]<<endl;
  
  // 遍历可以通过迭代器
  // 迭代器可以理解为指向pair的指针，first是key，second是value
  map<int, int>:: iterator itr; 
  unordered_map<int, int>:: iterator unordered_itr; 
  for (itr = my_map.begin(); itr != my_map.end(); itr++) { 
      cout << itr->first << "  " << itr->second << endl; 
  } 
  ```

  

### Complexity Analysis: 

- Time complexity : $O(n)$. 
- Space complexity : $O(1)$. 

### Details:

> Runtime: 4 ms, faster than 99.99% of C++ online submissions for Two Sum.
>
> Memory Usage: 10.3 MB, less than 42.46% of C++ online submissions for Two Sum.

## Solution 3: One-pass Hash Table

### Idea: 

同样使用哈希表，但只扫描一遍。因为每个数字只使用一次，所以我们在加入时，就检查能够与这个数字组成_target_的数字是否存在，如果存在则输出，否则加入到哈希表中。这种情况巧妙地避免了判断是否重复，因为在检查时，数字还没有被加入哈希表。只有当两个不在同一轮的数字匹配时才结束算法。

### Code: 

```c++
class Solution {
public:
    vector<int> twoSum(vector<int>& nums, int target) {
        unordered_map<int, int> hash_map;
        vector<int> ret(2);
        for (int i=0; i<nums.size(); i++) {
            if(hash_map.find(target - nums[i]) != hash_map.end()) {
                // 注意赋值顺序
                // 哈希表中存在的元素的下标一定比当前考察的元素下标小
                // 因此ret[0]为哈希表中元素下边，ret[1]为当前下标
                ret[0] = hash_map[target - nums[i]]; ret[1] = i;
                return ret;
            }
            else hash_map.insert(pair<int, int> (nums[i], i));
        }
        return {};
    }
};
```

### Rethinking:

- 一定要先确认匹配元素不在哈希表中之后，再将当前元素加入到哈希表中，这样可以避免一个数字被多次使用的情况。

  

### Complexity Analysis: 

- Time complexity : $O(n)$. 
- Space complexity : $O(1)$. 

### Details:

> Runtime: 4 ms, faster than 99.99% of C++ online submissions for Two Sum.
>
> Memory Usage: 10.2 MB, less than 40.15% of C++ online submissions for Two Sum.