using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Parser;

namespace LogParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Arquivo();
        }

        private static void Arquivo()
        {
            //Leitura do Arquivo
            String[] sLine = File.ReadAllLines(@"D:\Projetos\LogGame\Parser\Parser\games.log");
            //Instancia uma lista de games que ira armazenar a estrutura do game_n: { total_kills: 45; players: ["Dono da bola", "Isgalamido", "Zeh"] kills: { "Dono da bola": 5, "Isgalamido": 18, "Zeh": 20 }}
            List<Game> games = new List<Game>();

            Game game = null;
            //Conta o tamanho do arquivo e vai gravar a estrutura por game_n
            for (int i = 0; i < sLine.Length; i++)
            {
                //Faz a verificação ate o inicio do jogo
                string line = sLine[i].Substring(7);
                if (line.Contains("InitGame"))
                {
                    //Instancia a classe game, lista de plays, dicionario e adiciona na lista de games
                    game = new Game();
                    game.Players = new List<string>();
                    game.PlayerKills = new Dictionary<string, int>();
                    games.Add(game);
                }
                //verifica se a linha tem a entrada do usuario
                if (line.Contains("ClientUserinfoChanged"))
                {
                    //captura o nome do player
                    string player = line.Split(new char[] { Convert.ToChar(@"\") })[1];
                    //verifica se o player já existe, e adiciona, caso contrario não add, e como ele acabou de entrar ele inicia o kill como 0
                    if (!game.Players.Contains(player))
                    {
                        game.Players.Add(player);
                        game.PlayerKills.Add(player, 0);
                    }
                }
                //verifica se a linha tem uma morte.
                if (line.Substring(0, 4) == "Kill")
                {
                    //se tem kill então o total de kill add 1 e no proximo loop add +1
                    game.TotalKills += 1;
                    //da linha 61 a 79, verifica se tem espaço no nome do usuario e faz a uniao para capturar o usuário corretamente.
                    string[] data = line.Substring(line.IndexOf("killed ")).Split(' ');
                    string playerKilled = string.Empty;

                    for (int j = 0; j < data.Length; j++)
                    {
                        //se for nulo ou vazio ele adiciona o nome do usuario direto.
                        if (string.IsNullOrEmpty(playerKilled))
                        {
                            playerKilled += data[j + 1];
                        }
                        //se o nome tiver espaço ele vai add o espaço pra traser o usuario correto
                        else
                        {
                            playerKilled += " " + data[j + 1];
                        }
                        //verifca se o nome tem espaço, pelo tamanho do array.
                        if (data.Length == 4 || (data.Length > 4 && j + 1 == data.Length - 3))
                        {
                            break;
                        }
                    }
                    //se a morte for pelo world ele decremente 1
                    if (line.Contains("<world>"))
                    {
                        game.PlayerKills[playerKilled] -= 1;
                    }
                    //se a morte não for pelo world incremente 1
                    else
                    {
                        game.PlayerKills[playerKilled] += 1;
                    }
                }
            }
            //Criando strutura similar ao json            
            //Mostra Game por Game
            for (int i = 0; i < games.Count; i++)
            {
                string json = JsonConvert.SerializeObject(games[i], Formatting.Indented);  
                string Game = "Game_" + i + ": \n";
                Console.WriteLine(Game + json + "\n");
            }            

            Console.WriteLine("###### Relatório Mortes por Partida? ######  \n s - exibir / n - sair");

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.S)
            {
                Console.Clear();
                RelatorioKill();

            }
            else if (keyInfo.Key == ConsoleKey.N)
            {
                Environment.Exit(0);
            }

            Console.ReadKey();
        }

        private static void RelatorioKill()
        {
            String[] sLine = File.ReadAllLines(@"D:\Projetos\LogGame\Parser\Parser\games.log");
            List<GameKill> games = new List<GameKill>();

            GameKill game = null;
            for (int i = 0; i < sLine.Length; i++)
            {
                string line = sLine[i].Substring(7);
                if (line.Contains("InitGame"))
                {
                    game = new GameKill();
                    game.Kills = new List<string>();
                    game.MotivoKills = new Dictionary<string, int>();
                    games.Add(game);
                }
                if (line.Contains("Kill"))
                {
                    string[] kill = line.Substring(line.IndexOf("killed ")).Split(' ');
                    string kil = String.Empty;
                    switch (kill.Length)
                    {
                        case 3:
                            kil = kill[2];
                            break;
                        case 4:
                            kil = kill[3];
                            break;
                        case 5:
                            kil = kill[4];
                            break;
                        case 6:
                            kil = kill[5];
                            break;
                        case 7:
                            kil = kill[6];
                            break;
                        case 8:
                            kil = kill[7];
                            break;
                        case 9:
                            kil = kill[8];
                            break;
                        case 10:
                            kil = kill[9];
                            break;
                    }
                    if (!game.Kills.Contains(kil))
                    {
                        game.Kills.Add(kil);
                        game.MotivoKills.Add(kil, 0);
                    }
                }
                if (line.Substring(0, 4) == "Kill")
                {
                    game.TotalKills += 1;

                    string[] killed = line.Substring(line.IndexOf("killed ")).Split(' ');
                    string kil = String.Empty;
                    switch (killed.Length)
                    {
                        case 3:
                            kil = killed[2];
                            break;
                        case 4:
                            kil = killed[3];
                            break;
                        case 5:
                            kil = killed[4];
                            break;
                        case 6:
                            kil = killed[5];
                            break;
                        case 7:
                            kil = killed[6];
                            break;
                        case 8:
                            kil = killed[7];
                            break;
                        case 9:
                            kil = killed[8];
                            break;
                        case 10:
                            kil = killed[9];
                            break;
                    }
                    game.MotivoKills[kil] += 1;
                }
            }

            for (int i = 0; i < games.Count; i++)
            {
                string json = JsonConvert.SerializeObject(games[i], Formatting.Indented);            
                var Game = "Game_" + i + ": \n";
                Console.WriteLine(Game + json + "\n");
            }           
        }
    }
}
