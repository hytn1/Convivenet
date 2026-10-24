
using SQLite;
using Projeto_Faculdade.Models;
using Projeto_Faculdade.Services;

namespace Projeto_Faculdade.Data
{
    public class BancoDados
    {
        private readonly SQLiteConnection conexao;

        public BancoDados()
        {
            string caminho = Path.Combine(
                FileSystem.AppDataDirectory,
                "ProjetoFaculdade.db3"
            );

            conexao = new SQLiteConnection(caminho);

            // Tabelas
            conexao.CreateTable<Usuario>();
            conexao.CreateTable<Modulo>();
            conexao.CreateTable<ProgressoModulo>();
            conexao.CreateTable<Pergunta>();

            // Atualiza bancos antigos sem apagar dados
            AdicionarColunaFoto();
            AdicionarColunaImagem();

            // Cria e atualiza os módulos
            CriarModulos();

            // Cria as perguntas dos módulos
            CriarPerguntas();
        }

        private void AdicionarColunaFoto()
        {
            try
            {
                conexao.Execute(
                    "ALTER TABLE Usuario ADD COLUMN FotoPerfil TEXT"
                );
            }
            catch (SQLiteException)
            {
                // A coluna já existe.
            }
        }

        private void AdicionarColunaImagem()
        {
            try
            {
                conexao.Execute(
                    "ALTER TABLE Modulo ADD COLUMN Imagem TEXT"
                );
            }
            catch (SQLiteException)
            {
                // A coluna já existe.
            }
        }

        // =========================
        // USUÁRIOS
        // =========================

        public bool CadastrarUsuario(string nome, string senha)
        {
            Usuario? usuarioExistente = conexao.Table<Usuario>()
                .FirstOrDefault(u => u.Nome == nome);

            if (usuarioExistente != null)
                return false;

            Usuario novoUsuario = new Usuario
            {
                Nome = nome,
                SenhaHash = SenhaHelper.CriarHash(senha),
                Pontos = 0,
                Nivel = 1,
                FotoPerfil = ""
            };

            conexao.Insert(novoUsuario);

            return true;
        }

        public Usuario? FazerLogin(string nome, string senha)
        {
            Usuario? usuario = conexao.Table<Usuario>()
                .FirstOrDefault(u => u.Nome == nome);

            if (usuario == null)
                return null;

            bool senhaCorreta = SenhaHelper.VerificarSenha(
                senha,
                usuario.SenhaHash
            );

            return senhaCorreta ? usuario : null;
        }

        public Usuario? BuscarUsuarioPorId(int idUsuario)
        {
            return conexao.Table<Usuario>()
                .FirstOrDefault(u => u.Id == idUsuario);
        }

        public bool AtualizarFotoPerfil(int idUsuario, string caminhoFoto)
        {
            Usuario? usuario = BuscarUsuarioPorId(idUsuario);

            if (usuario == null)
                return false;

            usuario.FotoPerfil = caminhoFoto;
            conexao.Update(usuario);

            return true;
        }

        // MÓDULOS
       
        private void CriarModulos()
        {
            List<Modulo> modulos = new List<Modulo>
            {
                new Modulo
                {
                    Titulo = "Entendendo o Bullying",
                    Imagem = "modulo1.jpg",
                    Texto = "O bullying acontece quando uma pessoa ou um grupo pratica agressões repetidas contra alguém, causando sofrimento, medo ou constrangimento. Essas atitudes podem acontecer por meio de apelidos ofensivos, xingamentos, ameaças, empurrões, exclusão de brincadeiras ou comentários que fazem outra pessoa se sentir inferior.\n\nO bullying não é uma simples brincadeira. Quando alguém se sente machucado ou humilhado, é importante levar seus sentimentos a sério. Essas situações podem acontecer na escola, em outros ambientes sociais e até pela internet.\n\nCada pessoa possui suas próprias características, opiniões, sentimentos e dificuldades. Por isso, as diferenças devem ser respeitadas, e ninguém deve ser tratado de forma ruim por ser diferente dos outros.\n\nTodos nós podemos ajudar a prevenir o bullying por meio de atitudes como respeitar os colegas, pensar antes de falar, não participar de humilhações e acolher quem está passando por essa situação. Se você presenciar ou sofrer bullying, procure a ajuda de um adulto de confiança, como um professor ou responsável.\n\nLembre-se: ser diferente não é motivo para ser maltratado. Uma convivência saudável começa quando tratamos os outros com respeito, empatia e gentileza. Nossas palavras e atitudes podem machucar, mas também podem fazer alguém se sentir acolhido e valorizado.",
                    Ordem = 1
                },

                new Modulo
                {
                    Titulo = "Empatia e Respeito",
                    Imagem = "modulo2.jpg",
                    Texto = "A empatia é a capacidade de tentar compreender o que outra pessoa está sentindo, pensando ou vivendo. Ela nos ajuda a olhar as situações de diferentes pontos de vista e a perceber que cada pessoa pode reagir de uma maneira diferente diante do mesmo acontecimento.\n\nPraticar a empatia significa ouvir com atenção, tentar entender os sentimentos dos outros e oferecer ajuda quando alguém precisa. Por exemplo, se um colega estiver triste ou se sentir excluído de uma brincadeira, podemos conversar com ele, demonstrar carinho e convidá-lo para participar.\n\nO respeito está diretamente ligado à empatia, pois nos ensina a tratar as pessoas com educação, independentemente de suas características, opiniões ou diferenças. Mesmo quando não concordamos com alguém, podemos conversar sem ofender, humilhar ou desvalorizar seus sentimentos.\n\nA empatia e o respeito são atitudes importantes para evitar conflitos, fortalecer amizades e construir uma convivência mais saudável. Pequenas ações, como ouvir sem interromper, ajudar um colega e pensar antes de falar, podem fazer uma grande diferença no dia de alguém.\n\nLembre-se: antes de agir ou falar, tente pensar em como a outra pessoa pode se sentir. Quando procuramos compreender os sentimentos dos outros e os tratamos com respeito, contribuímos para um ambiente em que todos possam se sentir acolhidos, valorizados e seguros.",
                    Ordem = 2
                },

                new Modulo
                {
                    Titulo = "Como Agir Diante do Bullying",
                    Imagem = "modulo3.jpg",
                    Texto = "Saber como agir diante do bullying é muito importante para proteger a nós mesmos e ajudar outras pessoas. Quando presenciamos uma situação de bullying, podemos sentir medo, tristeza ou não saber o que fazer. Mesmo assim, existem atitudes que podem ajudar a interromper essas situações e tornar o ambiente mais seguro para todos.\n\nSe você estiver sofrendo bullying, lembre-se de que a culpa não é sua. Você não precisa enfrentar essa situação sozinho. Procure um adulto de confiança, como seus pais, um professor, um responsável ou alguém da coordenação da escola. Conte o que está acontecendo e explique como você se sente. Pedir ajuda é uma atitude de coragem, não de fraqueza.\n\nSe você presenciar alguém sofrendo bullying, não participe das provocações, não ria e não incentive as agressões. Essas atitudes podem fazer com que a pessoa se sinta ainda pior. Se for seguro, demonstre apoio à pessoa, converse com ela e ajude-a a procurar um adulto responsável. Você não precisa enfrentar quem pratica o bullying nem se colocar em perigo para ajudar.\n\nTambém é importante entender que compartilhar mensagens, fotos ou vídeos que humilham alguém pode aumentar o sofrimento da vítima. Quando o bullying acontece pela internet, evite espalhar esse conteúdo, guarde as informações necessárias para relatar o ocorrido e procure um adulto de confiança para receber orientação.\n\nQuem pratica bullying também precisa compreender que suas atitudes podem causar consequências e machucar outras pessoas. Reconhecer o erro, pedir desculpas sinceramente e mudar o próprio comportamento são passos importantes para aprender a conviver melhor. Porém, pedir desculpas não elimina a necessidade de interromper as agressões e respeitar os limites da outra pessoa.\n\nTodos podemos contribuir para combater o bullying por meio do respeito, da empatia e da responsabilidade. Pequenas atitudes, como acolher alguém que está triste, denunciar uma situação a um adulto e não participar de humilhações, podem fazer uma grande diferença.\n\nLembre-se: você não precisa resolver tudo sozinho. Pedir ajuda, apoiar quem precisa e escolher não participar das agressões são atitudes que ajudam a construir um ambiente mais respeitoso, seguro e acolhedor para todos.",
                    Ordem = 3
                },

                new Modulo
                {
                    Titulo = "Amizade, Inclusão e Convivência",
                    Imagem = "modulo4.jpg",
                    Texto = "A amizade é uma parte importante da nossa vida, pois nos permite compartilhar momentos, aprender coisas novas e criar lembranças especiais com outras pessoas. Uma amizade verdadeira é construída com confiança, respeito, carinho e sinceridade. Ser amigo não significa concordar com tudo, mas saber ouvir, compreender as diferenças e tratar o outro com consideração.\n\nA inclusão acontece quando todas as pessoas têm a oportunidade de participar, aprender, brincar e conviver juntas, sem serem deixadas de lado por causa de suas características, dificuldades, opiniões ou interesses. Cada pessoa tem seu próprio jeito de ser, e são justamente essas diferenças que tornam o mundo tão diverso. Ninguém deve ser excluído ou desvalorizado por ser diferente dos demais.\n\nNa escola, podemos praticar a inclusão com atitudes simples, como convidar um colega para participar de uma brincadeira, formar grupos sem deixar ninguém de fora, respeitar o tempo de aprendizagem de cada pessoa e ouvir as ideias dos outros. Às vezes, alguém pode ficar sozinho não porque deseja, mas porque tem vergonha, está triste ou não sabe como se aproximar. Um gesto de acolhimento pode fazer essa pessoa se sentir muito melhor.\n\nA convivência acontece todos os dias, em casa, na escola e em outros lugares onde encontramos pessoas. Para conviver bem, precisamos aprender a compartilhar espaços, respeitar regras, resolver desentendimentos por meio do diálogo e reconhecer que nossas atitudes afetam quem está ao nosso redor. Nem sempre vamos pensar da mesma maneira, mas podemos conversar com educação e procurar soluções sem ofender ou machucar ninguém.\n\nTambém é importante entender que amizade não combina com humilhação, ameaças ou pressão para fazer algo que não queremos. Um amigo de verdade respeita nossos sentimentos, nossos limites e nossas escolhas. Se alguém usa a amizade como desculpa para machucar, controlar ou excluir outra pessoa, esse comportamento precisa ser levado a sério e pode ser compartilhado com um adulto de confiança.\n\nCada um de nós pode ajudar a construir um ambiente mais acolhedor. Cumprimentar alguém, dividir materiais, ajudar um colega, respeitar as diferenças e não participar de brincadeiras ofensivas são atitudes que fortalecem a amizade e tornam a convivência melhor para todos. Não precisamos ser melhores amigos de todas as pessoas, mas devemos tratar cada uma com respeito e dignidade.\n\nLembre-se: incluir é abrir espaço, respeitar é reconhecer o valor de cada pessoa e conviver é aprender a compartilhar o mundo com os outros. Quando escolhemos agir com gentileza, empatia e responsabilidade, ajudamos a construir um lugar onde todos possam participar, fazer amizades e se sentir aceitos. Uma pequena atitude sua pode transformar o dia de alguém!",
                    Ordem = 4
                },

                new Modulo
                {
                    Titulo = "Resolução de Conflitos e Pedido de Ajuda",
                    Imagem = "modulo5.jpg",
                    Texto = "Conviver com outras pessoas é uma oportunidade de aprender, compartilhar experiências e construir amizades. Porém, é normal que, em alguns momentos, surjam desentendimentos. Podemos ter opiniões diferentes, querer brincar de maneiras distintas ou ficar chateados com alguma atitude. O mais importante não é nunca ter conflitos, mas aprender a resolvê-los de maneira respeitosa, sem machucar ou desvalorizar ninguém.\n\nQuando surgir um problema, procure manter a calma antes de reagir. Agir com raiva, gritar, ofender ou empurrar alguém pode piorar a situação. Em vez disso, tente explicar o que aconteceu e como você se sentiu, usando palavras respeitosas. Por exemplo, você pode dizer: 'Eu fiquei chateado quando você pegou meu material sem pedir. Da próxima vez, poderia me perguntar antes?'. Assim, você mostra seus sentimentos sem atacar a outra pessoa.\n\nOuvir também é uma parte importante da resolução de conflitos. Muitas vezes, cada pessoa entende uma situação de um jeito diferente. Por isso, vale a pena escutar o que o outro tem a dizer, fazer perguntas e tentar compreender seu ponto de vista. Isso não significa que você precisa concordar com tudo, mas que está disposto a conversar e buscar uma solução justa para todos os envolvidos.\n\nOutra atitude importante é reconhecer quando cometemos um erro. Se você magoar alguém, assumir a responsabilidade, pedir desculpas com sinceridade e procurar mudar seu comportamento demonstra maturidade. Da mesma forma, se alguém pedir desculpas, você pode ouvir o que essa pessoa tem a dizer. Entretanto, ninguém é obrigado a perdoar imediatamente, e a confiança pode precisar de tempo para ser reconstruída. O mais importante é que as atitudes que causaram o problema parem.\n\nExistem situações em que conversar sozinho não é suficiente. Se houver ameaças, agressões físicas, humilhações repetidas, exclusão intencional ou qualquer situação que provoque medo e sofrimento, é importante pedir ajuda a um adulto de confiança. Você pode procurar seus pais, responsáveis, professores ou a coordenação da escola. Conte o que aconteceu com o máximo de clareza possível e não tenha vergonha de pedir apoio. Procurar ajuda não é dedurar alguém por qualquer motivo, mas buscar uma solução segura para uma situação que precisa de atenção.\n\nSe você presenciar um conflito ou perceber que alguém está sofrendo, também pode ajudar. Evite incentivar discussões, espalhar fofocas ou compartilhar conteúdos que humilhem outras pessoas. Se for seguro, demonstre apoio a quem precisa e incentive essa pessoa a conversar com um adulto responsável. Nunca se coloque em perigo tentando separar uma briga ou enfrentar alguém que esteja sendo agressivo.\n\nLembre-se: resolver conflitos exige diálogo, paciência, respeito e responsabilidade. Nem sempre encontraremos uma solução imediatamente, mas podemos escolher agir de maneira mais consciente. E, quando uma situação for difícil demais para resolver sozinho, pedir ajuda é uma atitude de coragem. Todos merecem ser ouvidos, respeitados e tratados com dignidade.",
                    Ordem = 5
                }
            };

            foreach (Modulo moduloNovo in modulos)
            {
                Modulo? moduloExistente = conexao.Table<Modulo>()
                    .FirstOrDefault(m => m.Ordem == moduloNovo.Ordem);

                if (moduloExistente == null)
                {
                    conexao.Insert(moduloNovo);
                }
                else
                {
                    moduloExistente.Titulo = moduloNovo.Titulo;
                    moduloExistente.Texto = moduloNovo.Texto;
                    moduloExistente.Imagem = moduloNovo.Imagem;

                    conexao.Update(moduloExistente);
                }
            }
        }

        // =========================
        // PERGUNTAS
        // =========================

        private void CriarPerguntas()
        {
            conexao.DeleteAll<Pergunta>();

            List<Modulo> modulos = BuscarModulos();

            Modulo? mod1 = modulos.FirstOrDefault(m => m.Ordem == 1);
            Modulo? mod2 = modulos.FirstOrDefault(m => m.Ordem == 2);
            Modulo? mod3 = modulos.FirstOrDefault(m => m.Ordem == 3);
            Modulo? mod4 = modulos.FirstOrDefault(m => m.Ordem == 4);
            Modulo? mod5 = modulos.FirstOrDefault(m => m.Ordem == 5);

            List<Pergunta> perguntas = new List<Pergunta>();

            if (mod1 != null)
            {
                perguntas.Add(new Pergunta
                {
                    ModuloId = mod1.Id,
                    Texto = "Durante várias semanas, alguns colegas chamam Pedro por um apelido que ele não gosta. Mesmo depois de ele pedir para pararem, eles continuam e riem dele. O que Pedro ou seus colegas poderiam fazer nessa situação?",
                    AlternativaA = "Pedro deve aceitar o apelido para não parecer chato.",
                    AlternativaB = "Os colegas podem continuar, porque estão apenas brincando.",
                    AlternativaC = "Pedro pode expressar como se sente e procurar um adulto de confiança para ajudá-lo a interromper a situação.",
                    AlternativaD = "Pedro deve inventar apelidos para os colegas também.",
                    RespostaCorreta = "C",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod1.Id,
                    Texto = "Na hora do recreio, Ana esconde o estojo de Lucas uma vez, achando que seria engraçado. Lucas fica chateado, e Ana percebe que ele não gostou. Qual seria a melhor atitude de Ana?",
                    AlternativaA = "Esconder o estojo novamente para fazer todos rirem.",
                    AlternativaB = "Devolver o estojo, pedir desculpas e não repetir a brincadeira.",
                    AlternativaC = "Dizer que Lucas não sabe brincar.",
                    AlternativaD = "Culpar outro colega pelo que aconteceu.",
                    RespostaCorreta = "B",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod1.Id,
                    Texto = "Durante uma atividade de matemática, Júlia erra uma resposta. Alguns colegas começam a rir e dizem que ela nunca vai aprender. O que os colegas deveriam fazer?",
                    AlternativaA = "Continuar rindo para que Júlia aprenda a não errar.",
                    AlternativaB = "Ignorar Júlia e deixar que ela resolva tudo sozinha.",
                    AlternativaC = "Dizer que ela deveria desistir da atividade.",
                    AlternativaD = "Respeitar Júlia e oferecer ajuda para que ela tente novamente.",
                    RespostaCorreta = "D",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod1.Id,
                    Texto = "Nas aulas de educação física, um grupo de crianças sempre deixa Rafael por último na escolha dos times e diz que ninguém quer jogar com ele. Isso acontece em todas as aulas. Qual atitude seria mais adequada?",
                    AlternativaA = "Conversar com o professor para organizar equipes de forma justa e respeitosa.",
                    AlternativaB = "Mandar Rafael jogar sozinho.",
                    AlternativaC = "Fingir que nada está acontecendo, mesmo percebendo que Rafael está triste.",
                    AlternativaD = "Combinar com os colegas de nunca escolher Rafael.",
                    RespostaCorreta = "A",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod1.Id,
                    Texto = "Marina e Beatriz discordam sobre as regras de um jogo. As duas ficam irritadas, discutem por alguns minutos e depois conseguem conversar. Como podemos entender essa situação?",
                    AlternativaA = "Toda discussão entre amigos é bullying.",
                    AlternativaB = "Beatriz deve sempre concordar com Marina.",
                    AlternativaC = "Um desentendimento pode acontecer entre pessoas, e elas podem resolvê-lo conversando com respeito.",
                    AlternativaD = "Marina e Beatriz nunca mais devem brincar juntas.",
                    RespostaCorreta = "C",
                    Pontos = 10
                });
            }

            if (mod2 != null)
            {
                perguntas.Add(new Pergunta
                {
                    ModuloId = mod2.Id,
                    Texto = "Um aluno novo chega à escola e passa o recreio sozinho porque ainda não conhece ninguém. Você percebe que ele parece tímido. O que você pode fazer?",
                    AlternativaA = "Esperar que ele descubra sozinho como fazer amigos.",
                    AlternativaB = "Convidá-lo para brincar e ajudá-lo a conhecer os outros colegas.",
                    AlternativaC = "Fazer perguntas sobre ele na frente de todos para chamar atenção.",
                    AlternativaD = "Dizer que ele precisa mudar seu jeito de ser para conseguir amigos.",
                    RespostaCorreta = "B",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod2.Id,
                    Texto = "Na sala, Caio gosta de desenhar e prefere atividades tranquilas. Alguns colegas dizem que isso é estranho porque todos deveriam gostar de futebol. Qual atitude demonstra respeito?",
                    AlternativaA = "Obrigar Caio a jogar futebol para fazer parte do grupo.",
                    AlternativaB = "Dizer que os colegas estão certos porque todos precisam gostar das mesmas coisas.",
                    AlternativaC = "Evitar conversar com Caio por causa de seus gostos.",
                    AlternativaD = "Entender que cada pessoa tem seus próprios interesses e respeitar as escolhas de Caio.",
                    RespostaCorreta = "D",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod2.Id,
                    Texto = "Sofia precisa apresentar um trabalho para a turma, mas fica nervosa e começa a falar baixinho. Alguns colegas percebem que ela está com vergonha. Como eles podem demonstrar empatia?",
                    AlternativaA = "Escutá-la com atenção e incentivá-la com palavras gentis.",
                    AlternativaB = "Imitar a voz de Sofia para fazer a turma rir.",
                    AlternativaC = "Interromper a apresentação para dizer que ela está falando errado.",
                    AlternativaD = "Pedir que Sofia saia da frente da turma.",
                    RespostaCorreta = "A",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod2.Id,
                    Texto = "Um aluno usa óculos e começa a ser chamado por apelidos desagradáveis relacionados à sua aparência. Qual é a atitude mais respeitosa?",
                    AlternativaA = "Inventar um apelido diferente para participar da brincadeira.",
                    AlternativaB = "Evitar ficar perto dele para não virar alvo também.",
                    AlternativaC = "Não usar apelidos ofensivos e ajudar a interromper a situação, procurando um adulto se necessário.",
                    AlternativaD = "Dizer que ele só precisa aprender a ignorar os comentários.",
                    RespostaCorreta = "C",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod2.Id,
                    Texto = "Depois de ouvir comentários desagradáveis de alguns colegas, Miguel fica quieto durante o recreio. Você percebe que ele não está agindo como de costume. O que seria uma boa atitude?",
                    AlternativaA = "Contar para toda a turma que Miguel está triste.",
                    AlternativaB = "Aproximar-se com cuidado, perguntar se ele está bem e oferecer ajuda.",
                    AlternativaC = "Dizer que ele está exagerando e precisa se animar.",
                    AlternativaD = "Ignorá-lo porque os problemas dele não são seus.",
                    RespostaCorreta = "B",
                    Pontos = 10
                });
            }

            if (mod3 != null)
            {
                perguntas.Add(new Pergunta
                {
                    ModuloId = mod3.Id,
                    Texto = "No recreio, você vê três colegas cercando um aluno e fazendo comentários para humilhá-lo. O aluno parece assustado. O que você deve fazer?",
                    AlternativaA = "Juntar-se ao grupo para não ficar de fora.",
                    AlternativaB = "Filmar ou registrar a situação para mostrar aos outros.",
                    AlternativaC = "Rir junto e depois fingir que não viu nada.",
                    AlternativaD = "Procurar um adulto de confiança e, se for seguro, apoiar o aluno que está sendo humilhado.",
                    RespostaCorreta = "D",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod3.Id,
                    Texto = "Uma colega conta que está sendo excluída e humilhada repetidamente. Ela pede que você não conte a ninguém porque tem medo de que a situação piore. Qual é a melhor atitude?",
                    AlternativaA = "Escutá-la, demonstrar apoio e explicar que vocês podem procurar juntos um adulto de confiança.",
                    AlternativaB = "Prometer que nunca contará a ninguém, mesmo que ela continue sofrendo.",
                    AlternativaC = "Dizer que ela precisa resolver tudo sozinha.",
                    AlternativaD = "Contar para todos os alunos da escola.",
                    RespostaCorreta = "A",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod3.Id,
                    Texto = "Alguns colegas começam a rir enquanto uma criança é alvo de provocações. Eles chamam você para rir também. Como você pode agir?",
                    AlternativaA = "Rir para não ser excluído do grupo.",
                    AlternativaB = "Repetir as provocações para mostrar que faz parte da turma.",
                    AlternativaC = "Não participar da humilhação e buscar uma maneira segura de apoiar a criança.",
                    AlternativaD = "Incentivar os colegas a continuarem porque a situação parece engraçada.",
                    RespostaCorreta = "C",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod3.Id,
                    Texto = "Você percebe que seu melhor amigo está colocando apelidos ofensivos em um colega todos os dias. Você gosta dele, mas sabe que isso está machucando outra pessoa. O que você pode fazer?",
                    AlternativaA = "Defender seu amigo, porque amizades devem estar acima de tudo.",
                    AlternativaB = "Conversar com ele com respeito, explicar que o comportamento machuca e procurar um adulto se continuar.",
                    AlternativaC = "Começar a ofender o mesmo colega para apoiar seu amigo.",
                    AlternativaD = "Romper imediatamente todas as amizades e não conversar com ninguém.",
                    RespostaCorreta = "B",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod3.Id,
                    Texto = "Você conta a um professor que um colega está sendo alvo de humilhações frequentes. O professor agradece por ter contado e diz que vai ajudar a investigar o que está acontecendo. Qual deve ser sua atitude?",
                    AlternativaA = "Retirar o que contou, mesmo sabendo que a situação acontece.",
                    AlternativaB = "Espalhar o assunto pela escola para que todos saibam quem está envolvido.",
                    AlternativaC = "Confrontar sozinho todos os colegas envolvidos.",
                    AlternativaD = "Continuar buscando ajuda dos adultos responsáveis e evitar espalhar comentários sobre o caso.",
                    RespostaCorreta = "D",
                    Pontos = 10
                });
            }

            if (mod4 != null)
            {
                perguntas.Add(new Pergunta
                {
                    ModuloId = mod4.Id,
                    Texto = "A professora pede que a turma forme grupos para um trabalho. Um aluno costuma ficar sozinho, e ninguém o convida para participar. O que você pode fazer?",
                    AlternativaA = "Convidá-lo para o grupo e dividir as tarefas para que todos possam contribuir.",
                    AlternativaB = "Deixar que ele faça o trabalho inteiro sozinho.",
                    AlternativaC = "Dizer que ele não pode participar porque ninguém o conhece bem.",
                    AlternativaD = "Pedir que ele fique esperando até todos os grupos estarem completos.",
                    RespostaCorreta = "A",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod4.Id,
                    Texto = "Durante uma brincadeira, algumas crianças dizem que uma colega não pode participar porque é menina. Qual seria a melhor resposta?",
                    AlternativaA = "Concordar, porque o grupo tem o direito de escolher quem pode brincar.",
                    AlternativaB = "Sugerir que ela assista à brincadeira, mas não participe.",
                    AlternativaC = "Explicar que todos devem ter a oportunidade de participar, desde que respeitem as regras do jogo.",
                    AlternativaD = "Propor uma nova brincadeira da qual ela também não possa participar.",
                    RespostaCorreta = "C",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod4.Id,
                    Texto = "Em um trabalho escolar, Lucas apresenta uma ideia, mas os colegas não prestam atenção. Quando outra criança sugere algo parecido, todos elogiam a proposta. Como o grupo deveria agir?",
                    AlternativaA = "Continuar ignorando Lucas porque a ideia já foi apresentada.",
                    AlternativaB = "Reconhecer a contribuição de Lucas e escutar as ideias de todos com atenção.",
                    AlternativaC = "Dizer que Lucas deveria falar mais alto para merecer ser ouvido.",
                    AlternativaD = "Escolher sempre a ideia da pessoa mais popular da turma.",
                    RespostaCorreta = "B",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod4.Id,
                    Texto = "No recreio, você está usando um jogo de tabuleiro e percebe que outras crianças também querem participar. Há espaço para mais jogadores. O que você pode fazer?",
                    AlternativaA = "Esconder o jogo para que ninguém mais possa brincar.",
                    AlternativaB = "Permitir a participação somente dos seus melhores amigos.",
                    AlternativaC = "Dizer que quem chegou depois não tem direito de participar, mesmo havendo espaço.",
                    AlternativaD = "Convidar as outras crianças, combinar as regras e organizar os turnos de participação.",
                    RespostaCorreta = "D",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod4.Id,
                    Texto = "Seu amigo perde uma partida durante uma brincadeira. Ele fica irritado e diz que não quer mais brincar com você porque você venceu. Qual é uma boa maneira de lidar com isso?",
                    AlternativaA = "Zombar dele porque você ganhou.",
                    AlternativaB = "Dizer que ele nunca mais poderá brincar com você.",
                    AlternativaC = "Conversar com calma, explicar que ganhar e perder fazem parte do jogo e dar um tempo para que ele se acalme.",
                    AlternativaD = "Inventar uma regra para que você sempre vença.",
                    RespostaCorreta = "C",
                    Pontos = 10
                });
            }

            if (mod5 != null)
            {
                perguntas.Add(new Pergunta
                {
                    ModuloId = mod5.Id,
                    Texto = "Durante o recreio, duas crianças querem usar a mesma bola ao mesmo tempo. Elas começam a discutir e nenhuma quer ceder. Qual é a melhor maneira de resolver a situação?",
                    AlternativaA = "Conversar, ouvir os dois lados e combinar uma forma justa de usar a bola.",
                    AlternativaB = "Pegar a bola escondido enquanto os dois discutem.",
                    AlternativaC = "Empurrar o colega para conseguir a bola primeiro.",
                    AlternativaD = "Desistir da brincadeira e culpar o outro por tudo.",
                    RespostaCorreta = "A",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod5.Id,
                    Texto = "Um colega faz um comentário desagradável sobre você. Você fica com muita raiva e sente vontade de empurrá-lo. O que seria mais adequado fazer?",
                    AlternativaA = "Empurrá-lo para mostrar que você não tem medo.",
                    AlternativaB = "Respirar fundo, afastar-se se necessário e procurar ajuda caso as provocações continuem.",
                    AlternativaC = "Reunir amigos para se vingarem juntos.",
                    AlternativaD = "Responder com ofensas ainda piores.",
                    RespostaCorreta = "B",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod5.Id,
                    Texto = "Um colega pede que você prometa guardar segredo sobre as agressões que sofre de outras crianças. Ele diz que tem medo de contar ao professor. Qual é a melhor atitude?",
                    AlternativaA = "Prometer silêncio e nunca falar sobre o assunto.",
                    AlternativaB = "Dizer que ele precisa aprender a se defender sozinho.",
                    AlternativaC = "Contar para todos os alunos para que eles possam decidir o que fazer.",
                    AlternativaD = "Escutá-lo com carinho e ajudá-lo a conversar com um adulto de confiança, explicando que a segurança dele é importante.",
                    RespostaCorreta = "D",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod5.Id,
                    Texto = "Você percebe que fez uma piada sobre um colega e que ele ficou triste. Você não pretendia machucá-lo, mas percebe que sua atitude teve esse efeito. O que você deveria fazer?",
                    AlternativaA = "Dizer que ele não sabe brincar e que o problema é dele.",
                    AlternativaB = "Continuar fazendo a piada para mostrar que não se importa.",
                    AlternativaC = "Pedir desculpas sinceramente, escutar como ele se sentiu e evitar repetir a atitude.",
                    AlternativaD = "Culpar os outros colegas por terem rido.",
                    RespostaCorreta = "C",
                    Pontos = 10
                });

                perguntas.Add(new Pergunta
                {
                    ModuloId = mod5.Id,
                    Texto = "Você já pediu a alguns colegas que parem de fazer comentários ofensivos sobre outra criança, mas eles continuam repetindo o comportamento e a criança está cada vez mais triste. O que você deve fazer agora?",
                    AlternativaA = "Procurar um professor, coordenador ou outro adulto de confiança e explicar o que está acontecendo.",
                    AlternativaB = "Continuar tentando resolver tudo sozinho, mesmo que a situação piore.",
                    AlternativaC = "Incentivar a criança a revidar com agressões.",
                    AlternativaD = "Ignorar a situação porque você já tentou ajudar uma vez.",
                    RespostaCorreta = "A",
                    Pontos = 10
                });
            }

            conexao.InsertAll(perguntas);
        }
       
        public bool ConcluirModulo(int usuarioId, int moduloId)
        {
            ProgressoModulo? progresso = conexao.Table<ProgressoModulo>()
                .FirstOrDefault(
                    p => p.UsuarioId == usuarioId &&
                         p.ModuloId == moduloId
                );

            if (progresso == null)
            {
                progresso = new ProgressoModulo
                {
                    UsuarioId = usuarioId,
                    ModuloId = moduloId,
                    Concluido = true
                };

                conexao.Insert(progresso);
            }
            else
            {
                progresso.Concluido = true;
                conexao.Update(progresso);
            }

            return true;
        }

        public List<Modulo> BuscarModulos()
        {
            return conexao.Table<Modulo>()
                .OrderBy(m => m.Ordem)
                .ToList();
        }

        public List<Pergunta> BuscarPerguntasDoModulo(int moduloId)
        {
            return conexao.Table<Pergunta>()
                .Where(p => p.ModuloId == moduloId)
                .ToList();
        }

        public List<ProgressoModulo> BuscarProgressoUsuario(int usuarioId)
        {
            return conexao.Table<ProgressoModulo>()
                .Where(p => p.UsuarioId == usuarioId)
                .ToList();
        }

        public void AdicionarPontos(int usuarioId, int pontos)
        {
            Usuario? usuario = BuscarUsuarioPorId(usuarioId);

            if (usuario == null)
                return;

            usuario.Pontos += pontos;

            // A cada 20 pontos, sobe um nível
            usuario.Nivel = (usuario.Pontos / 20) + 1;

            conexao.Update(usuario);
        }

        public bool ModuloEstaConcluido(int usuarioId, int moduloId)
        {
            ProgressoModulo? progresso = conexao
                .Table<ProgressoModulo>()
                .FirstOrDefault(p =>
                    p.UsuarioId == usuarioId &&
                    p.ModuloId == moduloId);

            return progresso != null && progresso.Concluido;
        }
    }
}
