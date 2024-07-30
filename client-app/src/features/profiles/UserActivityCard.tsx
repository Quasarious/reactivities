import React from "react";
import { UserActivity } from "../../app/models/profile";
import { observer } from "mobx-react-lite";
import { Card, Image } from "semantic-ui-react";
import { Link } from "react-router-dom";

interface Props {
    activity: UserActivity;
}

export default observer (function UserActivityCard({activity}: Props) {
    return (
        <Card as={Link} to={`/activities/${activity.id}`}>
            <Image 
                src={`/assets/categoryImages/${activity.category}.jpg`} 
                style={{ minHeight: 100, objectFit: 'cover' }}
            />
            <Card.Content>
                <Card.Header textAlign="center">{activity.title}</Card.Header>
                <Card.Meta textAlign="center">
                    <div>{new Date(activity.date).toLocaleDateString()}</div>
                    <div>{new Date(activity.date).toLocaleTimeString()}</div>
                </Card.Meta>
            </Card.Content>
        </Card>
    )
})