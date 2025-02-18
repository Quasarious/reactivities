import React from "react";
import { Tab } from "semantic-ui-react";
import ProfilePhotos from "./ProfilePhotos";
import { Profile } from "../../app/models/profile";
import { observer } from "mobx-react-lite";
import ProfileAboutContent from "./ProfileAboutContent";
import ProfileFollowings from "./ProfileFollowings";
import { useStore } from "../../app/stores/store";
import ProfileActivities from "./ProfileActivities";

interface Props {
    profile: Profile;
}

export default observer (function ProfileContent ({profile} : Props) {
    const {profileStore} = useStore();
    const panes = [
        {menuItem: 'Профиль', render: () => <ProfileAboutContent />},
        {menuItem: 'Фото', render: () => <ProfilePhotos profile={profile}/>},
        {menuItem: 'Мероприятия', render: () => <ProfileActivities username={profile.username}/>},
        {menuItem: 'Подписчики', render: () => <ProfileFollowings />},
        {menuItem: 'Подписки', render: () => <ProfileFollowings />},
    ];

    return (
        <Tab 
            menu={{fluid : true, vertical : true}}
            menuPosition='right'
            panes={panes}
            onTabChange={(e, data) => profileStore.setActiveTab(data.activeIndex)}
        />
    )
})