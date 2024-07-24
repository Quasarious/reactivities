import React from "react";
import { Tab, TabPane } from "semantic-ui-react";
import ProfilePhotos from "./ProfilePhotos";
import { Profile } from "../../app/models/profile";
import { observer } from "mobx-react-lite";

interface Props {
    profile: Profile;
}

export default observer (function ProfileContent ({profile} : Props) {
    const panes = [
        {menuItem: 'Обо мне', render: () => <TabPane>About Content</TabPane>},
        {menuItem: 'Фото', render: () => <ProfilePhotos profile={profile}/>},
        {menuItem: 'Мероприятия', render: () => <TabPane>Events Content</TabPane>},
        {menuItem: 'Подписчики', render: () => <TabPane>Followers Content</TabPane>},
        {menuItem: 'Подписки', render: () => <TabPane>Following Content</TabPane>},
    ];

    return (
        <Tab 
            menu={{fluid : true, vertical : true}}
            menuPosition='right'
            panes={panes}
        />
    )
})