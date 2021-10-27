//Views.py
class UnityDatabase(ModelViewSet):
    serializer_class = UnityDatabaseSerializer
    queryset = UnityModel.objects.all()
    
//Models.py
class UnityModel(models.Model):
    username = models.CharField(max_length = 20)
    score = models.CharField(max_length = 20)
    
//Urls.py
router = DefaultRouter()
router.register('unity',views.UnityDatabase)
//Dentro de urlpatterns
path('unitydatabase/',include(router.urls)),

//Serializers.py
class UnityDatabaseSerializer(serializers.ModelSerializer):
    class Meta:
        model = UnityModel
        fields = ('username','score')

//Admin.py
class UnityModelAdmin(admin.ModelAdmin):
    list_display = ['username', 'score']

admin.site.register(UnityModel, UnityModelAdmin)
