//OneNote 字符串->单词逆序输出
#include <iostream>
#include <algorithm>
#include <string>

using namespace std;

void strrev(string word, int j)
{
    reverse(word.begin(), word.begin()+j);
    int i=0;
    while(i < j)
    {
        cout<<word[i];
        i++;
    }
    cout<<' ';
}

int main()
{
    string s,word(500,'\0');
    int i=0,j=0;
    getline(cin, s);
    for(i=0;i<s.length();i++)
    {
        if(s[i]!=' ')
        {
            word[j]=s[i];
            j++;
        }
        else
        {
            strrev(word,j);
            j=0;
        }
    }
    strrev(word, j);
    return 0;
}
