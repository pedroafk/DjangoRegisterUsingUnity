# UnityAndDjangoCommunication
Comunicação do Unity com o Django utilizando Rest Framework

Resumo: Você preenche um form de registro no unity e ele automaticamente envia uma ativação do django para seu email e já insere o usuário no banco de dados.

Códigos desenvolvidos para enviar um json via Rest Framwork do unity para o django, e também para receber json do django para o unity.

A parte .py está sem os includes, os próprios modelos utilizados já dirão quais são quando forem feito os scritps.

O preenchimento do json via unity está sendo feito utilizando uma box de input dentro do jogo que é enviado para o servidor ao apertar
um botão depois de preenchido.

O método get via unity está sendo feita via chamada de função diretamente no script do objeto no jogo. Caso necessário só fazer
as implementações utilizando o input ou com o uso do código: StartCoroutine(funçãoCriada());
