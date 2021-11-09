//Registro automático com envio de ativação para email preenchendo form no unity e recebendo email e colocando no banco do django.
//Views.py
class UnityRegister(ModelViewSet):
    serializer_class = UnityRegisterSerializer
    queryset = UnityRegisterModel.objects.all()
    
    #Função nativa do ModelViewSet sobrecarregada
    def create(self, request, *args, **kwargs):
        serializer = self.get_serializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        self.unityNewUser(serializer.data,request)
        return Response(serializer.data, status=status.HTTP_201_CREATED)
    
    def unityNewUser(self,data,request):
        newUserData = {'username': data['username'], 'first_name': data['first_name'],
                   'last_name': data['last_name'], 'email': data['email'],
                   'is_active': False, 'groups': [{'name': ''}]}
        
        data = {}
        editionName = settings.APP_CONFIG['edition']
        template_name = 'registration/' + editionName.lower() + '/activation_email.html'
        data['response'] = 'Usuário registrado com sucesso. Você receberá um e-mail com um link para ativar sua conta.'
        try:
            """
            Usuário registrado anteriormente terá e-mail e grupo atualizados e receberá
            novo link de ativação.
            """
            user = User.objects.get(username = newUserData['username'])
        except User.DoesNotExist:
            user = User.objects.create_user(
                username = newUserData['username'],
                first_name = newUserData['first_name'],
                last_name = newUserData['last_name'],
                email = newUserData['email'],
                is_active = False,
                password=''
            )
        user.email = newUserData['email']
        group = None
        groups = newUserData['groups']
        group = Group.objects.get(name = groups[0]['name'])
        user.groups.add(group)
        user.save()
        message = render_to_string(template_name, {
            'protocol': 'http',
            'user': user,
            'domain': request.get_host(),
            'uid': urlsafe_base64_encode(force_bytes(user.pk)),
            'token': default_token_generator.make_token(user),
        })
        subject = 'Ative sua conta'
        send_mail(
            subject = subject,
            message = message,
            from_email = '',
            recipient_list = [user.email],
            fail_silently = False,
            )
        return redirect('')
    
//Models.py
class UnityRegisterModel(models.Model):
    username = models.CharField(max_length = 50)
    first_name = models.CharField(max_length=50)
    last_name = models.CharField(max_length=50)
    email = models.CharField(max_length=50)
    
//Urls.py
router = DefaultRouter()
router.register('score',views.UnityScore) //para get
router.register('register',views.UnityRegister) //para post

//Dentro de urlpatterns
 path('unity/',include(router.urls)),

//Serializers.py
class UnityRegisterSerializer(serializers.ModelSerializer):
    class Meta:
        model = UnityRegisterModel
        fields = ('username','first_name','last_name','email')

//Só é necessário para método get, Post não precisa de ter um model na pag de adm.
//Admin.py
class UnityModelAdmin(admin.ModelAdmin):
    list_display = ['username', 'score']

admin.site.register(UnityModel, UnityModelAdmin)
