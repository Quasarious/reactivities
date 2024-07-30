import { observer } from "mobx-react-lite";
import React, { SyntheticEvent, useEffect } from "react";
import { Card, Grid, Header, Tab, TabPane, TabProps } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import UserActivityCard from "./UserActivityCard";
import { UserActivity } from "../../app/models/profile";

const panes = [
    { menuItem: 'Предстоящие', pane: { key: 'future' } },
    { menuItem: 'Прошедшие', pane: { key: 'past' } },
    { menuItem: 'Организатор', pane: { key: 'hosting' } }
   ];

interface Props {
    username: string;
}

export default observer(function ProfileActivities({username} : Props) {
    const {profileStore: {loadUserActivities, userActivities, loadingActivities}} = useStore();

    useEffect(() => {
        loadUserActivities(username);
    }, [loadUserActivities, username]);       

    const handleTabChange = (e: SyntheticEvent, data: TabProps) => {
        loadUserActivities(username, panes[data.activeIndex as
       number].pane.key);
    };

    return (
        <TabPane loading={loadingActivities}>
            
            <Grid>
                <Grid.Column width={16}>
                    <Header floated='left' icon='calendar' content={'Мероприятия'} />
                </Grid.Column>
                <Grid.Column width={16}>
                    <Tab
                        panes={panes}
                        menu={{ secondary: true, pointing: true }}
                        onTabChange={(e, data) => handleTabChange(e, data)}
                    />
                    <br />
                    <Card.Group itemsPerRow={4}>
                        {userActivities.map((activity: UserActivity) => (
                            <UserActivityCard key={activity.id} activity={activity}/>
                        ))}
                    </Card.Group>
                </Grid.Column>
            </Grid>
        </TabPane>
    );
});