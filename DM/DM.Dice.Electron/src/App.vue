
<template>
    <v-app id="inspire">
        <v-navigation-drawer v-model="drawer"
                             app
                             clipped>
            <v-subheader class="mt-4 grey--text text--darken-1">GLOSSARY</v-subheader>
            <v-list dense>
                <v-list-item v-for="item in items"
                             :key="item.text"
                             link>
                    <v-list-item-action>
                        <v-icon>{{ item.icon }}</v-icon>
                    </v-list-item-action>
                    <v-list-item-content>
                        <v-list-item-title>
                            {{ item.text }}
                        </v-list-item-title>
                    </v-list-item-content>
                </v-list-item>
            </v-list>
        </v-navigation-drawer>

        <v-app-bar app
                   clipped-left
                   color="blue darken-2"
                   dense>
            <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
            <v-toolbar-title class="mr-12 align-center">
                <span class="title">Dungeon Master</span>
            </v-toolbar-title>
            <v-spacer />
        </v-app-bar>

        <v-content>
            <v-container>
                <v-item-group>
                    <v-row v-if="monsters">
                        <v-col v-for="(monster, i) in monstersMinusRaces.slice(minPage, maxPage)"
                               :key="i"
                               cols="12"
                               md="3">
                            <v-card v-if="monster.details">
                                <v-list-item three-line>
                                    <v-list-item-content>
                                        <div class="overline mb-4">{{ monster.details.data.alignment | uppercase }}</div>
                                        <v-list-item-title class="headline mb-1">{{ monster.name }}</v-list-item-title>
                                        <v-list-item-subtitle>{{ monster.details.data.type | capitalize }}</v-list-item-subtitle>

                                    </v-list-item-content>

                                    <v-list-item-avatar tile
                                                        size="80"
                                                        color="grey">
                                        <img :src="monster.image"
                                             alt="img">
                                    </v-list-item-avatar>
                                </v-list-item>
                                <v-card-text>
                                    <v-row align="center"
                                           justify="center">
                                        <v-btn-toggle>
                                            <v-btn>
                                                {{ monster.details.data.strength }}<br>STR
                                            </v-btn>
                                            <v-btn>
                                                {{ monster.details.data.dexterity }}<br>DEX
                                            </v-btn>
                                            <v-btn>
                                                {{ monster.details.data.constitution }}<br>CON
                                            </v-btn>
                                            <v-btn>
                                                {{ monster.details.data.intelligence }}<br>INT
                                            </v-btn>
                                            <v-btn>
                                                {{ monster.details.data.wisdom }}<br>WIS
                                            </v-btn>
                                            <v-btn>
                                                {{ monster.details.data.charisma }}<br>CHR
                                            </v-btn>
                                        </v-btn-toggle>
                                    </v-row>

                                </v-card-text>
                                <v-card-actions>
                                    <v-btn color="red lighten-1" text>
                                        <v-icon left>mdi-heart</v-icon>
                                        {{ monster.details.data.hit_points }}
                                    </v-btn>
                                    <v-btn color="blue lighten-1" text>
                                        <v-icon left>mdi-shield</v-icon>
                                        {{ monster.details.data.armor_class }}
                                    </v-btn>
                                    <v-btn color="yellow lighten-1" text>
                                        <v-icon left>mdi-run</v-icon>
                                        {{ monster.details.data.speed.walk }}
                                    </v-btn>
                                    <v-btn color="green lighten-1" text>
                                        <v-icon left>mdi-dice-d20</v-icon>
                                        {{ monster.details.data.hit_dice | lowercase  }}
                                    </v-btn>
                                </v-card-actions>
                            </v-card>
                        </v-col>
                    </v-row>
                </v-item-group>
                <v-pagination v-model="page" :total-visible="10"
                              :length="totalPage"></v-pagination>
            </v-container>
        </v-content>
    </v-app>
</template>

<script>
    import Vue2Filters from 'vue2-filters'
    import axios from 'axios'

    export default {
        props: {
            source: String,
        },
        data: () => ({
            page: 1,
            perPage: 12,
            drawer: null,
            monsters: null,
            items: [
                { icon: 'mdi-account-group', text: 'Classes' },
                { icon: 'mdi-sword', text: 'Equipment' },
                { icon: 'mdi-bee-flower', text: 'Monsters' },
                { icon: 'mdi-auto-fix', text: 'Spells' }
            ],
            items2: [
                { picture: 28, text: 'Joseph' },
                { picture: 38, text: 'Apple' },
                { picture: 48, text: 'Xbox Ahoy' },
                { picture: 58, text: 'Nokia' },
                { picture: 78, text: 'MKBHD' },
            ],
        }),
        created() {
            this.$vuetify.theme.dark = true
            axios
                .get('https://www.dnd5eapi.co/api/monsters')
                .then(response => {
                    this.monsters = response.data.results
                    this.monsters.forEach(function (k) {
                        axios
                            .get('https://www.dnd5eapi.co' + k.url)
                            .then(response => (k.details = response))
                        k.isActive = false
                        var newName = k.name.split(' ').join('-').toLowerCase();
                        console.log(newName)
                        newName = newName.replace('adult-', '').replace('ancient-', '');
                        k.image = 'https://www.aidedd.org/dnd/images/' + newName + '.jpg'
                    });
                })

        },
        mixins: [Vue2Filters.mixin],
        computed: {
            minPage: function () {
                return (this.page - 1) * this.perPage
            },
            maxPage: function () {
                return this.minPage + this.perPage
            },
            totalPage: function () {
                if (this.monsters) {
                    return Math.ceil(this.monstersMinusRaces.length / this.perPage)
                }
                return 1;
            },
            monstersMinusRaces: function () {
                return this.monsters.filter(x => {
                    if (x.details != null) {
                        console.log(x.details.data)
                        return !x.details.data.subtype.includes('any ') || !x.details.data.alignment.includes('any ');
                    }
                    return true;
                })
            }
        }
    }
</script>