# [2\. Add Two Numbers](https://leetcode.com/problems/add-two-numbers/)

Difficulty: **Medium**


You are given two **non-empty** linked lists representing two non-negative integers. The digits are stored in **reverse order** and each of their nodes contain a single digit. Add the two numbers and return it as a linked list.

You may assume the two numbers do not contain any leading zero, except the number 0 itself.

**Example:**

```
Input: (2 -> 4 -> 3) + (5 -> 6 -> 4)
Output: 7 -> 0 -> 8
Explanation: 342 + 465 = 807.
```



## Solution 1: Elementary Math 1

### Idea: 

类似归并排序算法中的merge过程，将两个链表按照每一位合并到同一个链表中，同时使用一个carry位来记录进位。

### Code: 

```c++
/**
 * Definition for singly-linked list.
 * struct ListNode {
 *     int val;
 *     ListNode *next;
 *     ListNode(int x) : val(x), next(NULL) {}
 * };
 */
class Solution {
public:
    ListNode* addTwoNumbers(ListNode* l1, ListNode* l2) {
        int carry = 0, sum=0;
        sum = l1->val + l2->val + carry;
        if(sum>=10) {
            sum -=10;
            carry = 1;
        }
        // 记住初始化结构体的方法
        // 如果使用dummy head的方法，可以不用在这里就进行一次赋值
        ListNode* pos = new ListNode(sum);
        ListNode* ret=pos;
        l1 = l1->next;
        l2 = l2->next;
        while(l1!=NULL && l2!= NULL) {
            sum = l1->val + l2->val + carry;
            if(sum>=10) {
                sum-=10;
                carry = 1;
            } else {
                carry = 0;
            }
            pos->next = new ListNode(sum);
            pos = pos -> next;
            l1 = l1->next;
            l2 = l2->next;
        }
        while(l1!=NULL) {
            sum = carry + l1->val;
            if(sum>=10) {
                sum-=10;
                carry = 1;
            } else {
                carry = 0;
            }
            pos->next = new ListNode(sum);
            pos = pos -> next;
            l1 = l1->next;
        }
        while(l2!=NULL) {
            sum = carry + l2->val;
            if(sum>=10) {
                sum-=10;
                carry = 1;
            } else {
                carry = 0;
            }
            pos->next = new ListNode(sum);
            pos = pos -> next;
            l2 = l2->next;
        }
        if(carry ==1) {
            // 很重要的一点，不要漏掉进位为1的情况
            pos->next = new ListNode(1);
        }
        return ret;
        
    }
};
```

### Rethinking:

这是我最初的代码，看了答案之后发现还有很多可以优化的地方：

- 当初写代码时，觉得最复杂的就是第一个节点的初始化。因为需要最终返回第一个节点的指针，所以如果不初始化，而在循环中申请新节点，则需要区别对待第一个节点和之后的节点。同时，如果初始化了一个结点，则结果中会多出一个初始化的节点，比如_2+3_，结果就会返回_5->0_的链表。
- 答案给出了一个解决方法，初始化一个空的链表头，然后在循环中对其_next_开始赋值，最终返回链表头的_next_指针即可。这样虽然多申请了一个元素，但巧妙地将链表初始化与赋值分离，代码也更加简洁，文中称该方法为_dummy head_。
- 如果两个链表不等长，最初的代码使用了先同时计算，最后分别判断的方法，这也是受到归并排序_merge_过程的影响，但其实如果只有一个链表为空，完全可以当成此时该链表的数值为0来计算，从而在一个循环中直接得到答案。
- 对于代码中_sum_的赋值，可以直接使用_sum%10_来完成，而_carry_位可以通过_sum/10_来赋值。
- 

### Complexity Analysis: 

- Time complexity : $O(\max(m, n))$. Assume that  _m_ and  _n_ represents the length of  _l1_ and _l2_ respectively, the algorithm above iterates at most  $O(\max(m, n))$ times.
- Space complexity : $O(\max(m, n))$. The length of the new list is at most $O(\max(m, n))$.

### Details:

> Runtime: 24 ms, faster than 97.01% of C++ online submissions for Add Two Numbers.
>
> Memory Usage: 10.5 MB, less than 59.44% of C++ online submissions for Add Two Numbers.



## Solution 2: Elementary Math 2

### Idea: 

使用上述优化策略，对Solution1中的代码进行优化后的结果。

### Code: 

```c++
/**
 * Definition for singly-linked list.
 * struct ListNode {
 *     int val;
 *     ListNode *next;
 *     ListNode(int x) : val(x), next(NULL) {}
 * };
 */
class Solution {
public:
    ListNode* addTwoNumbers(ListNode* l1, ListNode* l2) {
        // 设置dummyHead
        ListNode* dummyHead = new ListNode(0);
        // 初始化pos，这里要与前一行分开写
        ListNode* pos = dummyHead;
        int carry=0, sum, num1, num2;
        while(l1!=NULL || l2!=NULL) {
            // 三目运算符的使用
            num1 = (l1==NULL?0:l1->val);
            num2 = (l2==NULL?0:l2->val);
            sum = num1 + num2 + carry;
            // carry赋值
            carry = sum/10;
            // sum赋值
            pos->next = new ListNode(sum%10);
            pos = pos ->next;
            // 加入空指针判断
            if (l1!=NULL) l1 = l1->next;
            if (l2!=NULL) l2 = l2->next;
        }
        if(carry !=0) pos->next = new ListNode(1);
        // 注意使用dummyHead时，返回其next指针
        return dummyHead->next;
        
    }
};
```

### Rethinking:

这段代码在时间上与Solution 1一致，空间上多申请了一个结点，但简洁了许多，关键在于链表的_dummyHead_应用以及除法、取余和三目的使用。

### Complexity Analysis: 

- Time complexity : $O(\max(m, n))$. Assume that  _m_ and  _n_ represents the length of  _l1_ and _l2_ respectively, the algorithm above iterates at most  $O(\max(m, n))$ times.
- Space complexity : $O(\max(m, n))$. The length of the new list is at most ==$O(\max(m, n))$+1==.

### Details:

> Runtime: 20 ms, faster than 99.10% of C++ online submissions for Add Two Numbers.
>
> Memory Usage: 10.4 MB, less than 73.52% of C++ online submissions for Add Two Numbers.



## Solution 3: Elementary Math 3

### Idea: 

进一步优化，将末尾对carry的判断加入到循环中

### Code: 

```c++
/**
 * Definition for singly-linked list.
 * struct ListNode {
 *     int val;
 *     ListNode *next;
 *     ListNode(int x) : val(x), next(NULL) {}
 * };
 */
class Solution {
public:
    ListNode* addTwoNumbers(ListNode* l1, ListNode* l2) {
        ListNode* dummyHead = new ListNode(0);
        ListNode* pos = dummyHead;
        int carry=0, sum, num1, num2;
        // 注意这里条件的变化
        while(l1!=NULL || l2!=NULL || carry) {
            num1 = (l1==NULL?0:l1->val);
            num2 = (l2==NULL?0:l2->val);
            sum = num1 + num2 + carry;
            carry = sum/10;
            pos->next = new ListNode(sum%10);
            pos = pos ->next;
            if (l1!=NULL) l1 = l1->next;
            if (l2!=NULL) l2 = l2->next;
        }
        // 无需再对carry进行判断
        return dummyHead->next;
    }
};
```

### Rethinking:

这段代码将num1、num2和carry视为同等地位的三个数，只有当三个数都为0时才结束循环，十分巧妙地避开了最后对carry的判断。