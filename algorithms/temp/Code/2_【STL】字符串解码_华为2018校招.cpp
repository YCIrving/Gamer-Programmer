//OneNote STL->字符串解码

#include <iostream>
#include <string>
#include <vector>
#include <algorithm>
#define MAX 65535
using namespace std;

//定义比较函数
int cmp(const pair<string, int>& x, const pair<string, int>& y)
{
    if(x.second!=y.second)
        return x.second < y.second;
    else
        return x.first < y.first;
}

int main()
{
    string input,output;
    cin>>input;
    vector<pair<string, int> > myPairVector;

    int i,j;
    int tag=0;
    //用来指示目前输入的读取状态
    int num;
    string str_letter;
    char num_array[MAX];
    //分析输入
    for (i=0;i<input.length();i++)
    {
        if (input[i]>='0'&&input[i]<='9')
        {
            num_array[0]=input[i];
            for (j=i+1;j<input.length();j++)
            {
                if(input[j]>='0'&&input[j]<='9')
                {
                    num_array[j-i]=input[j];
                }
                else
                {
                    num_array[j-i]='\0';
                    break;
                }
            }
            num=atoi(num_array);
            i=j-1;
            tag=0;
        }
        else
        {
            char str_array[MAX];
            str_array[0]=input[i];
            for (j=i+1;j<input.length();j++)
            {
                if(!(input[j]>='0'&&input[j]<='9'))
                {
                    str_array[j-i]=input[j];
                }
                else
                {
                    str_array[j-i]='\0';
                    break;
                }
            }
            str_letter=str_array;
            i=j-1;
            tag=1;
        }
        //如果读取完一遍字母+数字，则进行push_back
        if(tag==0)
        {
            myPairVector.push_back(make_pair(str_letter, num));
        }
    }

    //排序
    sort(myPairVector.begin(), myPairVector.end(), cmp);

    //输出
    int times=0;
    for(int i=0;i<myPairVector.size();i++)
    {
        times=myPairVector[i].second;
        for(j=0;j<times;j++)
            cout<<myPairVector[i].first;
    }
    return 0;
}
