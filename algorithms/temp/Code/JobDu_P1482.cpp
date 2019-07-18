//数据规模较小，所以建表，但是WA了，原因是把问题想得太简单了
//如果输入这样的数据，如20100002，则这个程序结果为-1，但是实际上为4；
/*
#include<iostream>
#include<string>
using namespace std;
string getLast4chars(string s,int n)
{
    string output="0000";
    output[0]=s[n-4];
    output[1]=s[n-3];
    output[2]=s[n-2];
    output[3]=s[n-1];
    output[4]='\0';
    return output;
}
int main()
{
    string s[12]={"2012","2021","2102","2120","2201","2210",
    "0212","0221","0122","1202","1220","1022"};
    int num[12]={0,1,1,2,2,3,1,2,2,2,3,3};
    int n,times;
    string input,temp;
    while(cin>>n)
    {
        times=-1;
        cin>>input;
        if(n<4)
        {
            cout<<-1<<endl;
        }
        else
        {
        int i;
        for(i=4;i<=n;i++)
        {
            temp=getLast4chars(input,i);
            if(temp=="2012")
            {
                cout<<0<<endl;
                break;
            }
            for(int j=1;j<12;j++)
            {
                if(s[j]==temp)
                {
                    if(times==-1)
                        times=num[j];
                    else
                    {
                        if(times>num[j])
                            times=num[j];
                    }
                }
            }
        }
        if(i>n)
            cout<<times<<endl;
        }
    }
    return 0;
}
*/
/*
说一下答案的思路，就是宽搜：
首先把字符串两两交换位置一次的字符串全部存到队列里，同时把交换次数也存至队列里
然后从队列头提出一个，再进行同样的操作，直到遍历完所有可能的结果。
此处要注意，交换时有可能遇到重复的情况，所以引入了一个map结构，用来排重。
具体的还没看太明白。
*/
#include <stdio.h>
#include <stdlib.h>
#include <string>
#include <string.h>
#include <queue>
#include <map>
using namespace std;
map<string , int> visitedMap;
map<string,int>::iterator it;
int n;
struct Node{
    char input[14];
    int step;
};
string str;
Node node;
void swapArr(char *input, int a, int b) {
    if (input[a] == input[b]) {
        return;
    }
    char temp = input[a];
    input[a] = input[b];
    input[b] = temp;
}
bool judgeIs2012(char *input) {
    for (int i = 0; i < n-3; i++) {
        if (input[i] == '2' && input[i+1] == '0'
                && input[i+2] == '1' && input[i+3] == '2') {
            return true;
        }
    }
    return false;
}
int bfs() {
    queue<Node> q ;
    while(!q.empty()) q.pop();
    q.push(node);
    while (!q.empty()) {
        node = q.front();
        q.pop();
        if (judgeIs2012(node.input)) {
            return node.step;
        }
        str = node.input;
        it = visitedMap.find(node.input);
        if (it == visitedMap.end() || it->second == 0) {
            visitedMap.insert(make_pair(str,1));
        }
        char *newInput;
        newInput = new char[str.size()];
        for (int i = 0; i < n-1; i++) {
            strcpy(newInput,node.input);
            swapArr(newInput,i,i+1);
            str = newInput;
            it = visitedMap.find(str);
            if (it == visitedMap.end() || it->second == 0) {
                visitedMap.insert(make_pair(str,1));
                Node temp;
                temp.step = node.step+1;
                strcpy(temp.input,newInput);
                q.push(temp);
            }
        }
    }
    return -1;
}
int main()
{
    while(scanf("%d",&n) != EOF ){
        char input[14];
        scanf("%s",input);
        if(n < 4){
            printf("-1\n");
            continue;
        }
        visitedMap.clear();
        node.step = 0;
        strcpy(node.input,input);
        printf("%d\n",bfs());
    }
    return 0;
}
