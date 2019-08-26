# 基础字符串操作

## split
```c++
template <class Container>
void split3(const std::string& str, Container& cont,
              char delim = ' ')
{
    std::size_t current, previous = 0;
    current = str.find(delim);
    while (current != std::string::npos) {
        cont.push_back(str.substr(previous, current - previous));
        previous = current + 1;
        current = str.find(delim, previous);
    }
    cont.push_back(str.substr(previous, current - previous));
}

int main()
{
    string s = "alice,20,800,mtv";
    vector<string> vec;
    split3(s,vec, ',');
    for(int i=0; i<vec.size(); i++)
    {
        cout<<vec[i]<<endl;
    }
    return 0;
}
```