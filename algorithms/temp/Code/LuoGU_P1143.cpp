/*
这道题是目前在洛谷上做的最复杂的一道入门难度的试题了。
思路很简单，就是先转换为十进制然后再用倒取余数法求出对应进制的数字。
敲完之后感觉代码可优化的空间还是很大的。
另外，一位同学的代码我放在注释里了，算法应该没有问题，就是不过（33分)，可能是C++在处理
数据时的各种隐式类型转换弄得/(ㄒoㄒ)/~~
*/
#include<iostream>
#include<string>
#include<math.h>
using namespace std;
int main()
{
    string input,mid;
    long int temp=0;
    double a;
    int n,m;//输入的是n进制，输出m进制
    char output[100]={'\0'};
    int counter=0;
    cin>>n>>input>>m;
    for(int i=input.length()-1;i>=0;i--)
    {
        switch (input[i])
        {
            case 'A':
            a=10;
            break;
            case 'B':
            a=11;
            break;
            case 'C':
            a=12;
            break;
            case 'D':
            a=13;
            break;
            case 'E':
            a=14;
            break;
            case 'F':
            a=15;
            break;
            default:
                a=input[i]-'0';
        };
        temp+=a*pow(n,input.length()-1-i);
    }

   while(temp>m)
   {
       switch(temp%m)
       {
        case 10:
        output[counter]='A';
        break;
        case 11:
        output[counter]='B';
        break;
        case 12:
        output[counter]='C';
        break;
        case 13:
        output[counter]='D';
        break;
        case 14:
       output[counter]='E';
        break;
        case 15:
        output[counter]='F';
        break;
        default:
        output[counter]=temp%m+'0';
       }
       temp/=m;
       counter++;
   }
   switch(temp)
       {
       case 10:
        output[counter]='A';
        break;
        case 11:
        output[counter]='B';
        break;
        case 12:
        output[counter]='C';
        break;
        case 13:
        output[counter]='D';
        break;
        case 14:
        output[counter]='E';
        break;
        case 15:
        output[counter]='F';
        break;
        default:
        output[counter]=temp+'0';
       };
       for(int i=counter;i>=0;i--)
       {
           cout<<output[i];
       }
    return 0;
}
/*leimingmark 33分
# include<iostream>
# include<string.h>
# include<math.h>
using namespace std;
#define N 1000000
void iniToeve(int initial, int tra)//此算法没问题
{
	if (initial / tra)
		iniToeve(initial / tra, tra);
	if (initial % tra == 10)
		cout << 'A';
	else if (initial % tra == 11)
		cout << 'B';
	else if (initial % tra == 12)
		cout << 'C';
	else if (initial % tra == 13)
		cout << 'D';
	else if (initial % tra == 14)
		cout << 'E';
	else if (initial % tra == 15)
		cout << 'F';
	else
		cout << initial % tra;
}
int main()
{
	int n;//最初的数的进制数
	cin >> n;
	char cha[N];//输入的数（字符形式存储）
	cin >> cha;
	int ch_length = strlen(cha);//字符数组的长度
	int temp = ch_length - 1;
	int sum = 0;//输入的数的十进制大小
	for (int i = 0; i < ch_length; i++)
	{
		switch (cha[i])//错误应该在这里
		{
		case '0':
			sum = sum + 0 * pow(n, temp);
			break;
		case '1':
			sum = sum + 1 * pow(n, temp);
			break;
		case '2':
			sum = sum + 2 * pow(n, temp);
			break;
		case '3':
			sum = sum + 3 * pow(n, temp);
			break;
		case '4':
			sum = sum + 4 * pow(n, temp);
			break;
		case '5':
			sum = sum + 5 * pow(n, temp);
			break;
		case '6':
			sum = sum + 6 * pow(n, temp);
			break;
		case '7':
			sum = sum + 7 * pow(n, temp);
			break;
		case '8':
			sum = sum + 8 * pow(n, temp);
			break;
		case '9':
			sum = sum + 9 * pow(n, temp);
			break;
		case 'A':
			sum = sum + 10 * pow(n, temp);
			break;
		case 'B':
			sum = sum + 11 * pow(n, temp);
			break;
		case 'C':
			sum = sum + 12 * pow(n, temp);
			break;
		case 'D':
			sum = sum + 13 * pow(n, temp);
			break;
		case 'E':
			sum = sum + 14 * pow(n, temp);
			break;
		case 'F':
			sum = sum + 15 * pow(n, temp);
			break;
		default:
			break;
		}
		temp--;
	}
	int m;
	cin >> m;
	iniToeve(sum, m);
	return 0;
}

评测结果 Result
    测试点 #1：通过该测试点。 得分16，耗时0ms，内存3092kB。
    测试点 #2：错误的答案。 得分0，耗时0ms，内存3088kB。
该行正确答案长度：10 你的答案长度：10
你是在整个测试点输出的 0% 地方开始出错的。
这一行你是在第 10 个字符开始与标准输出不同的。
    测试点 #3：错误的答案。 得分0，耗时0ms，内存3088kB。
该行正确答案长度：30 你的答案长度：30
你是在整个测试点输出的 0% 地方开始出错的。
这一行你是在第 29 个字符开始与标准输出不同的。
    测试点 #4：错误的答案。 得分0，耗时15ms，内存3096kB。
该行正确答案长度：8 你的答案长度：8
你是在整个测试点输出的 0% 地方开始出错的。
这一行你是在第 6 个字符开始与标准输出不同的。
    测试点 #5：错误的答案。 得分0，耗时0ms，内存3092kB。
该行正确答案长度：13 你的答案长度：13
你是在整个测试点输出的 0% 地方开始出错的。
这一行你是在第 13 个字符开始与标准输出不同的。
    测试点 #6：通过该测试点。 得分17，耗时15ms，内存3092kB。
*/
