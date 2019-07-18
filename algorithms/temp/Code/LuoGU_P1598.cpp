/*
这道题目难度也不大，也是有两点需要注意一下
1.如何控制输出的行数是一个重点，这里定义了一个max，表示所有字母中出现最多的次数，
这样就能控制输出了，另外为了array的自减操作也需要多留意下;
2.最后就是空格的问题，以前一直以为输出必须跟答案完全一样才行，其实不然，这道题目可以看出
最后多余个空格是可以被忽略的（详见程序末尾的两种输出），所以只要不把正确答案覆盖掉，
或者添加多余可显示字符，其余的空行空格等都可以加。
*/

#include<iostream>
#include<string>
using namespace std;
int main()
{
    string temp;
    int array[26]={0};
    int max=0;
    for(int i=0;i<4;i++)
    {
        getline(cin,temp);
        for(int j=0;j<temp.length();j++)
        {
            if(temp[j]>='A'&&temp[j]<='Z')
            {
                array[temp[j]-'A']++;
                if(array[temp[j]-'A']>max)
                    max=array[temp[j]-'A'];
            }
        }
    }
    while(max>0)
    {
        for(int i=0;i<26;i++)
        {
            if(array[i]==max)
            {
                cout<<'*';
                array[i]--;//容易漏掉，这里需要注意
            }
            else
                cout<<' ';
            cout<<' ';
        }
        cout<<endl;
        max--;
    }
    for(int i=0;i<26;i++)
    {
        cout<<(char)(i+'A')<<' ';
    }
    //cout<<"A B C D E F G H I J K L M N O P Q R S T U V W X Y Z"<<endl;
    return 0;
}
