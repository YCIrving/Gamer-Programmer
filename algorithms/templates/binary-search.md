# 二分查找
二分查找的思想不难，但要真正在短时间内编写一个没有bug的二分查找是一件很难的事情，主要原因包括：选择左右中位数，分析返回left还是right，循环体内条件的判断，只有一个数是否成立，边界条件的判断等，这里在[Leetcode35](https://leetcode-cn.com/problems/search-insert-position/)的中文题解中介绍了一种[神奇的二分查找模板](https://leetcode-cn.com/problems/search-insert-position/solution/te-bie-hao-yong-de-er-fen-cha-fa-fa-mo-ban-python-/)，可以很好地解决上面的问题，下面对其内容和应用做个记录。

