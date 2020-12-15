# Sample Licensing System [![MIT license](https://img.shields.io/badge/License-MIT-blue.svg)](https://lbesson.mit-license.org/)
Made by <a href="https://github.com/hexck">Hexk</a>
<br><br>

## :smirk_cat: Why do you need Sample ? 

Sample was built so that people can learn from it, for real-world use, anti-tampering techniques and obfuscation would probably be required.
<br><br>

## :star: How does it work ?

- Client side (.NET Framework)
- Server side (.NET Core)
- MongoDB
<br>

## :fire: What does it do ?

- [x] All data is encrypted (RSA, AES)
- [x] Uses sockets
- [x] Easy to understand and implement
- [x] Command-line interface
- [x] Bans & License expiration

<br>

## :bookmark_tabs: Examples
```c#
    var sample = new SampleClient("127.0.0.1", 3202).Connect();
    if (!sample.EncryptConnection())
        return;

    while (!sample.Whitelisted())
    {
        Console.Write("You are not whitelisted, please enter license key: ");
        var res = sample.Register(Console.ReadLine());
        Console.WriteLine(res == RequestState.Success ? "Key was valid" : "Key was not valid");
    }
    sample.Close();
            
    Console.WriteLine("Gg! you are verified!");
```
