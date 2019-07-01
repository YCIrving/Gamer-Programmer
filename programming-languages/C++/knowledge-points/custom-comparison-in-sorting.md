# C++中比较函数comp()的应用

总结一下排序过程中常见自定义比较函数的使用方式。

1. `sort()`函数

    `sort()`是使用自定义比较函数最常见的情况，先看一下[官方文档](http://www.cplusplus.com/reference/algorithm/sort/)上的定义：
    
    ```c++
    // default (1)	
    template <class RandomAccessIterator>
    void sort (RandomAccessIterator first, RandomAccessIterator last);
    // custom (2)	
    template <class RandomAccessIterator, class Compare>
    void sort (RandomAccessIterator first, RandomAccessIterator last, Compare comp);
    ```
    
    >Sort elements in range Sorts the elements in the range [first,last) into **ascending** order. The elements are compared using **operator<** for the first version, and comp for the second.

    > Compare comp: Binary function that accepts two elements in the range as arguments, and returns a value convertible to bool. **The value returned indicates whether the element passed as first argument is considered to go before the second** in the specific strict weak ordering it defines.The function shall not modify any of its arguments.This can either be a function pointer or a function object.

    可见，默认的`sort()`是使用小于号来进行比较，而自定义排序时，`comp()`需要返回一个布尔值，布尔值的含义是：`comp()`的第一个参数是否要放在第二个参数之前。所以，默认的比较中，如果使用小于号，则说明是升序排列，即第一个参数小于第二个参数时，返回true。

    因此衍生出两种自定义`sort()`的方式：

    - 自定义`comp()`的降序排列：

        ```c++
        int comp(int a, int b)
        {
            return a>b;
            // 等价于：
            // if(a>b) return true;
            // return false;
        }
        ```

        这里的代码还是比较好记的，返回值的形式跟最终的排列形式一致。对结构体的排序也可以通过类似写法实现。

    - 重载"<"的降序排序：
    
        **该方法仅限于对非基本类型进行排序，如结构体或类等**。重载直接在类型中定义：

        ```c++
        struct Student
        {
            char name[101];
            int age;
            int score;
            // 重载小于号
            // 记忆点1：需要两个const
            bool operator < (const Student &B) const 
            {
                if (score!=B.score)
                {
                    // 将参数当成比较函数中的第二个参数
                    return score>B.score;
                }
                else
                {
                    return strcmp(name,B.name);
                }
            }
        }students[1000];


        // 重载也可以写到结构体外面：
        struct Student
        {
            int score;
            // 重载小于号
        }students[1000];

        bool operator < (const Student &A, const Student &B)
        {
            return A.score<B.score;
        }

        ```

    注：`sort()`是不稳定的排序，时间复杂度为$O(N*log(N))$，如果需要稳定排序，可以使用`stable_sort()`，复杂度不变。

2. `priority_queue`

    要使用优先队列，就必须要对其中的排序规则做到精准的把控，还是先来看一下[官方文档](http://www.cplusplus.com/reference/queue/priority_queue/)：

    ```c++
    template <class T, class Container = vector<T>,
    class Compare = less<typename Container::value_type> > class priority_queue;
    ```

    >Priority queues are a type of container adaptors, specifically designed such that its first element is always the greatest of the elements it contains, according to some strict weak ordering criterion.

    >This context is similar to a heap, where elements can be inserted at any moment, and only the max heap element can be retrieved (the one at the top in the priority queue).

    >class Compare: A binary predicate that takes two elements (of type T) as arguments and returns a bool.
    >The expression comp(a,b), where comp is an object of this type and a and b are elements in the container, shall **return true if a is considered to go before b** in the strict weak ordering the function defines.
    >The priority_queue uses this function to maintain the elements sorted in a way that preserves heap properties (i.e., that **the element popped is the last** according to this strict weak ordering).
    >
    >This can be a function pointer or a function object, and defaults to `less<T>`, which returns the same as applying the less-than operator (a<b).

    从上面我们可以了解到：
    - `priority_queue`是一个类，而`sort()`是一个函数，所以对`priority_queue`的自定义排序需要在`priority_queue`被实例化时就完成，声明方式需要记忆，共三个参数，类型、迭代器和用来比较的类，如`priority_queue <int, vector<int>, greater<int> min_heap`;

    - `priority_queue`的自定义比较是一个`Compare`类，而`sort()`中是一个函数，所以二者不能直接转换，但只需要改动一点点，将`sort()`中的比较函数写在一个结构体中的**重载括号函数**中即可：
        ```c++
        struct comp
        {
            bool operator()(Student a,Student b)
            {
                return a.age>b.age;
            }
        };
        ```
        之后这样定义优先队列`priority_queue<Student, vector<Student>, comp> my_heap;`

    - 《王道机试》上有这样一句话：
        >(重载小于号)虽然与定义`comp`函数类似，我个人还是建议使用重载小于号。这样首先可以对重载运算符的写法有一定的了解；其次在今后使用标准模板库时，该写法也有一定的用处。

        推荐记忆重载小于号的写法的原因就是，我们可以很方便地将`sort()`和优先队列的自定义进行统一，比如我们可以在结构体中重载了小于号，就可以这样实例化优先队列：
        ```c++
        priority_queue<Student, vector<Student>, less<Student> > my_heap;
        // 由于实例化默认是less，还可以直接写成：
        // priority_queue<Student> my_heap;
        ```

        当然，也可以重载大于号，实例化时将`less`改为`greater`即可。

        **总之，对于优先队列，基本类型用`greater`或`less`，非基本类型用重载小于号是最简单的写法**。
    - 最后，最最重要的一点，**`priority_queue`每次弹出的是队尾的元素，即如果把其理解为一个堆的话，堆顶的元素实际上是排序后最后的一个元素**。这样记忆之后，就能理解为什么使用`less<int>`自定义排序后，建立的是一个大顶堆了，因为排序后最大的元素在队尾。