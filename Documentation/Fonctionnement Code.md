# Fonctionnement du code

Pour résumer le fonctionnement de notre application, la classe Game1 délègue au Level la création et l'utilisation des différentes ressources. Celui-ci va alors créer des instances pour les classes Bakground, Bike et RoadConstructor. Ainsi, il va faire en sorte d'afficher le fond d'écran, la moto ainsi que la route. De plus, suivant la position de la route et de la moto, il va savoir si une collision est présente entre ces deux. Dans ce cas, la gravité de la moto est nulle, sinon celle-ci est positive, pour que la moto descende. Ensuite, il sait également à l'avance la route est ascendante, et donc il mettra une gravité négative pour élever la moto.