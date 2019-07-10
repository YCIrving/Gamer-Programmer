```c++

// 7
class Solution {
public:
    int reverse(int x) {
        int rev=0;
        while(x!=0)
        {
            // 由于-2^31^=-2147483648,2^31^-1=2147483647,所以在反转的时候要判断是否溢出，主要检查反转时的最后一位和倒数第二位
            if(rev>INT_MAX/10 || rev==INT_MAX/10&& x%10 > 7) //判断上限
                return 0;
            if(rev<INT_MIN/10 || rev==INT_MIN/10&& x%10 < -8) //判断下限
                return 0;

            rev = rev *10 + x%10;
            x /=10;
        }
        return rev;
    }
};

// 9
class Solution {
public:
    bool isPalindrome(int x) {
        if(x<0 || x%10 == 0&&x!=0 ) return false;
        int rev = 0;
        while(rev<x)
        {
            rev = rev * 10 + x%10;
            x /= 10;
        }
        return rev==x || rev/10 ==x;
    }
};

// strcpy
char * strcpy(char *dst,const char *src)   //[1]
{
    assert(dst != NULL && src != NULL);    //[2]

    char *ret = dst;  //[3]

    while ((*dst++=*src++)!='\0'); //[4]

    return ret;
}


```