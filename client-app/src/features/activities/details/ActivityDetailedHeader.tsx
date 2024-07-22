import { observer } from 'mobx-react-lite';
import React from 'react';
import { Segment, Item, Header, Image, Button, Label } from 'semantic-ui-react';
import { Activity } from '../../../app/models/activity';
import { Link } from 'react-router-dom';
import { format } from 'date-fns';
import { useStore } from '../../../app/stores/store';


const activityImageStyle = {
    filter: 'brightness(30%)'
};

const activityImageTextStyle = {
    position: 'absolute',
    bottom: '5%',
    left: '5%',
    width: '100%',
    height: 'auto',
    color: 'white'
};

interface Props {
    activity: Activity
}

export default observer (function ActivityDetailedHeader({activity}: Props) {
    const {activityStore: {updateAttendance, loading, cancelActivityToggle}} = useStore();
    return (
        <Segment.Group>
            <Segment basic attached='top' style={{padding: '0'}}>
                {activity.isCanceled &&
                    <Label style={{position: 'absolute', zIndex: 1000, left: -14, top: 20}} ribbon color='red' content='Отменено' />
                }
                <Image src={`/assets/categoryImages/${activity.category}.jpg`} fluid style={activityImageStyle}/>
                <Segment style={activityImageTextStyle} basic>
                    <Item.Group>
                        <Item>
                            <Item.Content>
                                <Header
                                    size='huge'
                                    content={activity.title}
                                    style={{color: 'white'}}
                                />
                                <p>{format(activity.date!, 'dd MMMM yyyy')}</p>
                                <p>
                                    Hosted by <strong><Link to={`/profiles/${activity.hostUsername}`}>{activity.host?.displayName}</Link></strong>
                                </p>
                            </Item.Content>
                        </Item>
                    </Item.Group>
                </Segment>
            </Segment>
            <Segment clearing attached='bottom'>
                {activity.isHost ? (
                    <>
                        <Button 
                            color={activity.isCanceled ? 'green' : 'red'}
                            floated='left'
                            basic
                            content={activity.isCanceled ? 'Отменить отмену' : 'Отменить мероприятие'}
                            onClick={cancelActivityToggle}
                            loading={loading}
                        />
                        <Button as={Link} 
                            disabled={activity.isCanceled}
                            to={`/manage/${activity.id}`} 
                            color='orange' floated='right'>
                            Управление мероприятием
                        </Button>
                    </>
                ) : activity.isGoing? (
                    <Button loading={loading} onClick={updateAttendance}>Отменить участие</Button>
                ) : (
                    <Button 
                        disabled={activity.isCanceled}
                        loading={loading} 
                        onClick={updateAttendance} 
                        color='teal'>
                        Участвовать
                    </Button>
                )}
                
                
                
            </Segment>
        </Segment.Group>
    )
})


